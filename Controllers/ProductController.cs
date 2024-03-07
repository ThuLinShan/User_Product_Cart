using AutoMapper;
using ExcelDataReader;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.Product;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Helpers;
using User_Product_Cart.Interface;
using User_Product_Cart.Repository;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProduct _productRepository;
        private IMapper _mapper = ProductMapperConfig.InitializeAutomapper();
        //Constructor
        public ProductController(IProduct productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet] //Get all
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var response = await _productRepository.GetProducts();
                return Ok(response._productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRecentProducts")] //Get all
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        public async Task<IActionResult> GetRecentProducts()
        {
            try
            {
                var response = await _productRepository.GetRecentProducts();
                return Ok(response._productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{productId}")] //Get product by Id
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetProduct(productId);
                return StatusCode(product.StatusCode, product._productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Create Product
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductRequest addProduct)
        { 
            if (addProduct == null)
            {
                return BadRequest("Product is empty");
            }
            var response = await _productRepository.CreateProduct(addProduct);
            if (response.StatusCode == 500)
            {
                ModelState.AddModelError("", "Something went wrong while creating new product");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok("Product created successfully.");
            }
        }

        //Update Product
        [HttpPut("productId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest updateProduct, int productId)
        {
            if (updateProduct == null)
            {
                return BadRequest(ModelState);
            }
            var response = await _productRepository.UpdateProduct(updateProduct, productId);
            if (response.StatusCode == 500)
            {
                ModelState.AddModelError("", "Something went wrong while updating new product" + response.message);
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok("Product updated successfully.");
            }
        }

        //Delete Product
        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var response = await _productRepository.DeleteProduct(productId);
            if (response.StatusCode == 500)
            {
                ModelState.AddModelError("", "Something went wrong deleting product");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok("Product deleted successfully");
            }
        }

        //Create Product By Excel Datasheet
        [HttpPost("ProductExcelToDatabase")]
        public async Task<IActionResult> CreateProductExcel(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ProductResponse response = new ProductResponse();
            if(file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";
                if(!Directory.Exists(uploadsFolder)) 
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, file.Name);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        AddProductRequest addProduct = new AddProductRequest();
                        do
                        {
                            bool headerSkipper = true;
                            while (reader.Read())
                            {
                                if (headerSkipper)
                                {
                                    headerSkipper = false;
                                    continue;
                                }
                                addProduct.product_name = reader.GetValue(0).ToString();
                                addProduct.price_per_item = Convert.ToInt32(reader.GetValue(1).ToString());
                                addProduct.category = reader.GetValue(2).ToString();
                                addProduct.created_date = DateTime.Now;
                                await _productRepository.CreateProduct(addProduct);
                            }
                        } while (reader.NextResult());
                    }
                }

                return Ok("File uploaded to database successfully.");
            }
            else
            {
                return BadRequest("Please upload a file.");
            }
        }

        [HttpGet("ProductDatabaseToExcel")] //Get product and Export with excel datasheet
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductExcel()
        {
            try
            {
                var response = await _productRepository.GetProducts();
                ExcelMapper excelMapper = new ExcelMapper();
                var newFile = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Exports\\{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.xlsx";
                excelMapper.Save(newFile, response._productDtos, "SheetName", true);
                return Ok("Excel file created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ProductNamesDatabaseToExcel")] //Get product and Export with excel datasheet
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductNamesExcel()
        {
            try
            {
                var response = await _productRepository.GetProductNames();
                ExcelMapper excelMapper = new ExcelMapper();
                var newFile = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Exports\\{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}NameOnly.xlsx";
                excelMapper.Save(newFile, response._productNameDtos, "SheetName", true);
                return Ok("Excel file created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

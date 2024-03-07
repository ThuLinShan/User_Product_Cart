using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using User_Product_Cart.Dtos.Brand;
using User_Product_Cart.Interface;
using User_Product_Cart.Repository;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController: Controller
    {
        private readonly IBrand _brandRepository;
        public BrandController(IBrand brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpGet("GetBrandById")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var response = await _brandRepository.GetBrandById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetBrandList")]
        public async Task<IActionResult> GetBrandList()
        {
            var response = await _brandRepository.GetBrandList();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateBrand")]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequest req)
        {
            var response = await _brandRepository.CreateBrand(req);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddProductToBrand")]
        public async Task<IActionResult> AddProductToBrand(int productId, int brandId)
        {
            var response = await _brandRepository.AddProductToBrand(productId, brandId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandRequest req)
        {
            var response = await _brandRepository.UpdateBrand(req);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("RemoveBrand")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var response = await _brandRepository.DeleteBrand(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}

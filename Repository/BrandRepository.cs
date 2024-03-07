using Microsoft.EntityFrameworkCore;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Brand;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Repository
{
    public class BrandRepository : IBrand
    {
        private DataContext _context;
        public BrandRepository(DataContext context)
        {
            _context = context;
        }

        #region Add Product To Brand
        public async Task<ResponseStatus> AddProductToBrand(int productId, int brandId)
        {
            if (_context.Products.Where(p => p.Id == productId).Any())
            {
                if (_context.Brands.Where(b => b.Id == brandId).Any())
                {
                    await _context.BrandProducts.AddAsync(new BrandProduct { BrandId = brandId, ProductId = productId });
                    await _context.SaveChangesAsync();
                    return (new ResponseStatus
                    {
                        message = "Product is added to category successfully.",
                        StatusCode = StatusCodes.Status200OK
                    });
                }
                return (new ResponseStatus
                {
                    message = "Category not found",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return (new ResponseStatus
            {
                message = "Product not found",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }
        #endregion

        #region CreateBrand
        public async Task<ResponseStatus> CreateBrand(CreateBrandRequest req)
        {
            try
            {
                Brand brand = new Brand();
                brand.Name = req.Name;
                brand.Description = req.Description;
                await _context.Brands.AddAsync(brand);
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "New Brand Created Successfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region DeleteBrand
        public async Task<ResponseStatus> DeleteBrand(int id)
        {
            try
            {
                var brand = await _context.Brands.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (brand == null)
                    return (new ResponseStatus
                    {
                        message = "Brand Not Found with BrandId: " + id,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                brand.isActive = false;
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "Brand Deleted Successfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region Get Brand By Id
        public async Task<BrandResponse> GetBrandById(int id)
        {
            try
            {
                var response = await _context.Brands.Where(b => b.Id == id && b.isActive == true)
                                                               .Select(b => new BrandResponse
                                                               {
                                                                   Name = b.Name,
                                                                   Description = b.Description,
                                                               })
                                                               .FirstOrDefaultAsync();
                if (response == null)
                {
                    return (new BrandResponse
                    {
                        message = "Brand Not Found with Id: " + id,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.message = "Success";
                return response;
            }
            catch (Exception ex)
            {
                return (new BrandResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region Get Brand List
        public async Task<GetBrandListResponse> GetBrandList()
        {
            try
            {
                var response = new GetBrandListResponse();
                response.brands = await _context.Brands
                    .Where(b => b.isActive == true)
                    .Select(b => new BrandListItem
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                    })
                    .Take(16)
                    .ToListAsync();
                response.message = "Success";
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return (new GetBrandListResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region Update Brand
        public async Task<ResponseStatus> UpdateBrand(UpdateBrandRequest req)
        {
            try
            {
                var brand = await _context.Brands.Where(b => b.Id == req.Id).FirstOrDefaultAsync();
                if (brand == null)
                    return (new ResponseStatus
                    {
                        message = "Brand Not Found with BrandId: " + req.Id,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                brand.Name = req.Name;
                brand.Description = req.Description;
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "Brand Updated Successfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion
    }
}

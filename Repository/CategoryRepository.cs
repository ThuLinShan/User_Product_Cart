using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Ocsp;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Category;
using User_Product_Cart.Dtos.Product;
using User_Product_Cart.Dtos.Promotion;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace User_Product_Cart.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<ResponseStatus> CreateCategory(CreateCategoryRequest req)
        {
            try
            {
                Category category = new Category();
                category.Name = req.Name;
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "New Category Created Successfully",
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

        #region Delete
        public async Task<ResponseStatus> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (category == null)
                    return (new ResponseStatus
                    {
                        message = "Category Not Found with CategoryId: " + id,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                category.isActive = false;
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "Category Deleted Successfully",
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

        #region Add Product to Category
        public async Task<ResponseStatus> AddProductToCategory(int productId, int categoryId)
        {
            if (_context.Products.Where(p => p.Id == productId).Any())
            {
                if (_context.Categories.Where(c => c.Id == categoryId).Any())
                {
                    await _context.CategoryProducts.AddAsync(new CategoryProduct { CategoryId = categoryId, ProductId = productId });
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

        #region GetCategorizedProducts
        public async Task<GetMainCategoryProducts> GetCategorizedProducts()
        {
            try
            {
                var response = new GetMainCategoryProducts();
                response.MainCategories = await _context.Categories
                    .Where(c => c.ParentCategoryId == null)
                    .Select(c => new MainCategoryList
                    {
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        CategoryProducts  = _context.CategoryProducts
                                            .Where(cp => ((cp.Category.ParentCategoryId == null?(cp.CategoryId ==c.Id): cp.Category.ParentCategoryId == c.Id )))
                                            .OrderBy(cp => cp.Product.created_date)
                                            .Select(cp => new CategoryProductItem
                                            {
                                                productId       = cp.ProductId,
                                                productName     = cp.Product.product_name,
                                                categoryId      = cp.CategoryId,
                                                parentCategoryId = cp.Category.ParentCategoryId,
                                                parentCategoryName = c.Name,
                                                categoryName    = cp.Category.Name,
                                                created_date    = cp.Product.created_date,
                                                stock           = cp.Product.stock
                                            })
                                            .ToList()
                    })
                    .ToListAsync();
                response.message = "Success";
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return (new GetMainCategoryProducts
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region GetById
        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            try
            {
                var response = await _context.Categories.Where(c => c.Id == id && c.isActive == true)
                                                               .Select(c => new CategoryResponse
                                                               {
                                                                   Name = c.Name,
                                                                   parentId = c.ParentCategoryId
                                                               })
                                                               .FirstOrDefaultAsync();
                if (response == null)
                {
                    return (new CategoryResponse
                    {
                        message = "Category Not Found with Id: " + id,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.message = "Success";
                return response;
            }
            catch (Exception ex)
            {
                return (new CategoryResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region GetMainCategory
        public async Task<MainCategoryListResponse> GetMainCategory()
        {
            try
            {
                var response = new MainCategoryListResponse();
                response.mainCategories = await _context.Categories.Where(c => c.ParentCategoryId == null && c.isActive == true)
                                                               .Select(c => new MainCategories
                                                               {
                                                                   Id = c.Id,
                                                                   Name = c.Name,
                                                                   childCategories = _context.Categories
                                                                                    .Where(child => child.ParentCategoryId == c.Id)
                                                                                    .Select(child => new ChildCategories
                                                                                    {
                                                                                        Id= child.Id,
                                                                                        Name = child.Name,
                                                                                        parentId = child.ParentCategoryId
                                                                                    })
                                                                                    .ToList()
                                                               })
                                                        .ToListAsync();
                response.StatusCode = StatusCodes.Status200OK;
                response.message = "Success";
                return response;
            }
            catch (Exception ex)
            {
                return (new MainCategoryListResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region GetList
        public async Task<GetCategoryListResponse> GetCategoryList()
        {
            try
            {
                var response = new GetCategoryListResponse();
                response.ItemCount = await _context.Categories
                    .Where(c => c.isActive == true)
                    .CountAsync();
                response.Categories = await _context.Categories
                    .Where(c => c.isActive == true)
                    .Select(c => new CategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        parentId = c.ParentCategoryId
                    })
                    .ToListAsync();
                response.message = "Success";
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return (new GetCategoryListResponse
                { 
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        #endregion

        #region Update
        public async Task<ResponseStatus> UpdateCategory(int id, UpdateCategoryRequest req)
        {
            try
            {
                var category = await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (category == null)
                    return (new ResponseStatus
                    {
                        message = "Category Not Found with CategoryId: " + id,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                category.Name = req.Name;
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "Category Updated Successfully",
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

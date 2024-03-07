using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using System.Diagnostics;
using System.Transactions;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.Product;
using User_Product_Cart.Dtos.Promotion;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Helpers;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Repository
{
    public class ProductRepository : IProduct
    {
        ProductResponse productResponse = new ProductResponse();
        private readonly DataContext _context;
        private readonly IPromotion _promotionRepository;
        private IMapper _mapper = ProductMapperConfig.InitializeAutomapper();
        private readonly IMemoryCache _cache;
        private readonly string cacheKey = "productCacheKey";
        public ProductRepository(DataContext context, IMemoryCache cache, IPromotion promotionRepository)
        {
            _cache = cache;
            _context = context;
            _promotionRepository = promotionRepository;
        }


        public async Task<ProductResponse> GetProducts()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //To check if there are values in cache
                if(_cache.TryGetValue(cacheKey, out IEnumerable<ProductDto> products))
                {
                    Console.WriteLine("\nProduct Cache Found");
                }
                //If cache value is not available, get from database and create cache
                else
                {
                    Console.WriteLine("\nProducts Cache Not Found");
                    products = await _context.Products
                        .Select(p => new ProductDto
                        {
                             product_name = p.product_name,
                             price_per_item = p.price_per_item,
                             created_date = p.created_date,
                             stock = p.stock,
                             promotion = _context.Promotions
                                                         .Where(promotion => promotion.productId == p.Id && p.stock>0)
                                                         .Select(promotion => new ProductPromotionResponse
                                                         {
                                                             fixedAmount = promotion.fixedAmount,
                                                             percent = promotion.percent,
                                                             createdDate = promotion.createdDate,
                                                             createdBy = promotion.createdBy,
                                                             updatedDate = promotion.updatedDate,
                                                             updatedBy = promotion.updatedBy,
                                                             startDate = promotion.startDate,
                                                             endDate = promotion.endDate,
                                                             timeLimited = promotion.timeLimited
                                                         })
                                                         .AsNoTracking()
                                                         .FirstOrDefault()
                        })
                        .OrderBy(p => p.product_name)
                        .ToListAsync();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromHours(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(cacheKey, products, cacheEntryOptions);
                }
                stopwatch.Stop();
                Console.WriteLine("\n"+stopwatch.ElapsedMilliseconds+" passed");
                productResponse._productDtos = (List<ProductDto>)products;
                productResponse.StatusCode = 200;
                productResponse.message = "Success";
                return productResponse;
            }
            catch (Exception ex)
            {
                productResponse.StatusCode = 500;
                productResponse.message = ex.Message;
                return productResponse;
            }
        }

        public async Task<ProductResponse> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.Where(p => p.Id == id && p.stock > 0)
                    .Select(p => new ProductDto
                    {
                        product_name = p.product_name,
                        price_per_item = p.price_per_item,
                        created_date = p.created_date,
                        category = p.category,
                        stock = p.stock,
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    productResponse.productExists = false;
                    productResponse.StatusCode = 400;
                    productResponse.message = "Product not found";
                    return productResponse;
                }

                var promotion = await _context.Promotions
                    .Where(p => p.productId == id && ((p.timeLimited == true ? (p.startDate < DateTime.Now && p.endDate > DateTime.Now) : true)))
                    .Select(p => new ProductPromotionResponse
                    {
                        fixedAmount = p.fixedAmount ,
                        percent = p.percent         ,
                        createdDate = p.createdDate ,
                        createdBy = p.createdBy     ,
                        updatedDate = p.updatedDate ,
                        updatedBy = p.updatedBy     ,
                        startDate = p.startDate     ,
                        endDate = p.endDate         ,
                        timeLimited = p.timeLimited ,
                    })
                    .FirstOrDefaultAsync();

                product.promotion = promotion;

                productResponse._productDto = product;
                productResponse.StatusCode = StatusCodes.Status200OK;
                productResponse.message = "Success";
                return productResponse;
            }
            catch (Exception ex)
            {
                productResponse.StatusCode = 400;
                productResponse.message = ex.Message;
                return productResponse;
            }
        }

        public async Task<ResponseStatus> CreateProduct(AddProductRequest addProduct)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Product product = new Product();
                product.product_name = addProduct.product_name;
                product.price_per_item = addProduct.price_per_item;
                product.created_date = addProduct.created_date;
                product.category = addProduct.category;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                Console.WriteLine("Product save step finished.");

                if (addProduct.addPromotion != null)
                {
                    AddPromotionRequest promotion = new AddPromotionRequest();
                    promotion.productId = product.Id;
                    promotion.fixedAmount = addProduct.addPromotion.fixedAmount;
                    promotion.percent = addProduct.addPromotion.percent;
                    promotion.createdDate = addProduct.addPromotion.createdDate;
                    promotion.createdBy = addProduct.addPromotion.createdBy;
                    promotion.startDate = addProduct.addPromotion.startDate;
                    promotion.endDate = addProduct.addPromotion.endDate;
                    promotion.timeLimited = addProduct.addPromotion.timeLimited;

                    await _promotionRepository.AddPromotion(promotion);
                }

                Console.WriteLine("Promotion save step finished.");

                transaction.Commit();
                productResponse.StatusCode = 200;
                productResponse.message = "Success";
                return productResponse;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                productResponse.StatusCode = 500;
                productResponse.message = ex.Message;
                return productResponse;
            }

        }

        public async Task<ResponseStatus> UpdateProduct(UpdateProductRequest updateProduct, int productId)
        {
            try
            {
                Product product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
                #region Update product
                if (product == null)
                {
                    productResponse.productExists = false;
                    productResponse.StatusCode = 500;
                    productResponse.message = "Product Not Found";
                    return productResponse;
                }
                product.product_name = updateProduct.product_name;
                product.price_per_item = updateProduct.price_per_item;
                product.category = updateProduct.category;
                _context.SaveChanges();
                #endregion

                #region Update Promotion
                Promotion db_promotion = await _context.Promotions.Where(p => p.productId == productId).FirstOrDefaultAsync();
                if (updateProduct.updatePromotion == null)
                {
                    if(db_promotion != null)
                    {
                        await _promotionRepository.RemovePromotion(db_promotion.id);
                    }
                }
                else
                {
                    if (db_promotion == null)
                    {
                        AddPromotionRequest promotion = new AddPromotionRequest();
                        promotion.productId = productId;
                        promotion.fixedAmount = updateProduct.updatePromotion.fixedAmount;
                        promotion.percent = updateProduct.updatePromotion.percent;
                        promotion.createdDate = updateProduct.updatePromotion.Date;
                        promotion.createdBy = updateProduct.updatePromotion.By;
                        promotion.startDate = updateProduct.updatePromotion.startDate;
                        promotion.endDate = updateProduct.updatePromotion.endDate;
                        promotion.timeLimited = updateProduct.updatePromotion.timeLimited;
                        await _promotionRepository.AddPromotion(promotion);
                    }
                    else
                    {
                        UpdatePromotionRequest promotion = new UpdatePromotionRequest();
                        promotion.productId = productId;
                        promotion.fixedAmount = updateProduct.updatePromotion.fixedAmount;
                        promotion.percent = updateProduct.updatePromotion.percent;
                        promotion.updatedDate = updateProduct.updatePromotion.Date;
                        promotion.updatedBy = updateProduct.updatePromotion.By;
                        promotion.startDate = updateProduct.updatePromotion.startDate;
                        promotion.endDate = updateProduct.updatePromotion.endDate;
                        promotion.timeLimited = updateProduct.updatePromotion.timeLimited;
                        await _promotionRepository.UpdatePromotion(promotion);
                    }
                }

                #endregion
                productResponse.StatusCode = 200;
                productResponse.message = "User Updated Successfully";
                return productResponse;
            }
            catch (Exception ex)
            {
                productResponse.StatusCode = 500;
                productResponse.message = ex.Message;
                return productResponse;
            }
        }

        public async Task<ResponseStatus> DeleteProduct(int productId)
        {
            try
            {
                Product product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
                if (product == null)
                {
                    productResponse.productExists = false;
                    productResponse.StatusCode = 500;
                    productResponse.message = "Product Not Found";
                    return productResponse;
                }
                product.Id = productId;
                _context.Products.Remove(product);
                _context.SaveChanges();

                productResponse.StatusCode = 200;
                productResponse.message = "Success";
                return productResponse;
            }
            catch (Exception ex)
            {
                productResponse.StatusCode = 500;
                productResponse.message = "Failed";
                return productResponse;
            }
        }

        public async Task<ProductResponse> GetProductNames()
        {
            try
            {
                productResponse._productNameDtos = _mapper.Map<List<ProductNameDto>>(_context.Products.Where(p => p.stock > 0).OrderBy(p => p.product_name).ToList());
                productResponse.StatusCode = 200;
                productResponse.message = "Success";
                return productResponse;
            }
            catch (Exception ex)
            {
                productResponse.StatusCode = 500;
                productResponse.message = ex.Message;
                return productResponse;
            }
        }

        public async Task<ProductResponse> ProductExists(int productId)
        {
            productResponse.productExists = false;
            if (_context.Products.Any(p => p.Id == productId && p.stock > 0)) productResponse.productExists = true;
            return productResponse;
        }

        public void ClearCache()
        {
            _cache.Remove(cacheKey);
            Console.WriteLine("Cache has been cleared.");
        }

        public async Task<ProductResponse> GetRecentProducts()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => p.created_date >= DateTime.Now.AddDays(-5))
                    .Select(p => new ProductDto
                    {
                        product_name = p.product_name ,
                        price_per_item = p.price_per_item ,
                        category = p.CategoryProducts.Select(cp => cp.Category.Name).FirstOrDefault(),
                        created_date = p.created_date,
                        stock = p.stock,
                        promotion = _context.Promotions
                                            .Where(promotion => promotion.productId == p.Id && p.stock > 0)
                                            .Select(promotion => new ProductPromotionResponse
                                            {
                                                fixedAmount = promotion.fixedAmount,
                                                percent = promotion.percent,
                                                createdDate = promotion.createdDate,
                                                createdBy = promotion.createdBy,
                                                updatedDate = promotion.updatedDate,
                                                updatedBy = promotion.updatedBy,
                                                startDate = promotion.startDate,
                                                endDate = promotion.endDate,
                                                timeLimited = promotion.timeLimited
                                            })
                                            .AsNoTracking()
                                            .FirstOrDefault()
                    })
                    .OrderByDescending(p => p.created_date)
                    .ToListAsync();
                productResponse._productDtos = _mapper.Map<List<ProductDto>>(products);
                productResponse.StatusCode = 200;
                productResponse.message = "Success";
                return productResponse;
            }
            catch (Exception ex)
            {
                productResponse.StatusCode = 500;
                productResponse.message = ex.Message;
                return productResponse;
            }
        }
    }
}

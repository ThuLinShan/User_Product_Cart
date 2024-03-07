using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Promotion;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Repository
{
    public class PromotionRepository : IPromotion
    {
        private readonly DataContext _context;
        public PromotionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PromotionResponse> GetPromotion(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new PromotionResponse
                    {
                        message = "Empty input for creating promotion",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                var response = await _context.Promotions
                    .Where(p => p.id == id)
                    .Select(result => new PromotionResponse
                    {
                        id = result.id,
                        productId = result.productId,
                        fixedAmount = result.fixedAmount,
                        percent = result.percent,
                        createdDate = result.createdDate,
                        createdBy = result.createdBy,
                        updatedDate = result.updatedDate,
                        updatedBy = result.updatedBy,
                        startDate = result.startDate,
                        endDate = result.endDate,
                        timeLimited = result.timeLimited,
                        StatusCode =StatusCodes.Status200OK,
                        message = "Get Promotion by id success."
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (response == null)
                {
                    return new PromotionResponse
                    {
                        message = "Promotion Not Found.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                return response;
            }
            catch(Exception ex)
            {
                return new PromotionResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public async Task<PromotionListResponse> GetPromotions()
        {
            try
            {
                return new PromotionListResponse
                    {
                       promotions = await _context.Promotions
                                    .Where(promotion => promotion.Product.stock > 0 && ((promotion.timeLimited == true ? (promotion.startDate <= DateTime.Now && promotion.endDate >= DateTime.Now) : true)))
                                    .Select(promotion => new PromotionResponse
                                    {
                                        id              = promotion.id,
                                        productId       = promotion.productId,
                                        productName     = promotion.Product.product_name,
                                        fixedAmount     = promotion.fixedAmount,
                                        percent         = promotion.percent,
                                        defaultPrice    = promotion.Product.price_per_item,
                                        promotionPrice  = promotion.fixedAmount > 0 ? promotion.Product.price_per_item - promotion.fixedAmount : promotion.Product.price_per_item - (promotion.Product.price_per_item * promotion.percent / 100),
                                        differencePrice = promotion.fixedAmount > 0 ? promotion.fixedAmount : promotion.Product.price_per_item * promotion.percent / 100,
                                        createdDate     = promotion.createdDate,
                                        createdBy       = promotion.createdBy,
                                        updatedDate     = promotion.updatedDate,
                                        updatedBy       = promotion.updatedBy,
                                        startDate       = promotion.startDate,
                                        endDate         = promotion.endDate,
                                        timeLimited     = promotion.timeLimited,
                                        StatusCode      = StatusCodes.Status200OK,
                                        message         = "Get Promotion by id success."
                                    })
                                    .AsNoTracking()
                                    .ToListAsync(),
                        message = "Get Promotions Successfully",
                        StatusCode = StatusCodes.Status200OK
                };
            }
            catch( Exception ex )
            {
                return new PromotionListResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public async Task<ResponseStatus> AddPromotion(AddPromotionRequest addPromotion)
        {
            try
            {
                #region Validations
                if (addPromotion == null)
                {
                    return new ResponseStatus
                    {
                        message = "Empty input for creating promotion",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                var product = await _context.Products.Where(p => p.Id == addPromotion.productId).AsNoTracking().FirstOrDefaultAsync();
                if (product == null)
                {
                    return new ResponseStatus
                    {
                        message = "Product not found.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (_context.Promotions.Any(p => p.productId == addPromotion.productId))
                {
                    return new ResponseStatus
                    {
                        message = "Promotion already exists for product id: "+addPromotion.productId,
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if(addPromotion.fixedAmount < 0 || addPromotion.percent < 0 || addPromotion.percent > 100)
                {
                    return new ResponseStatus
                    {
                        message = "Please set at least one valid promotion type.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (addPromotion.fixedAmount > 0 && addPromotion.percent > 0)
                {
                    return new ResponseStatus
                    {
                        message = "Both promotion types cannot be set at the same time.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (product.price_per_item < addPromotion.fixedAmount)
                {
                    return new ResponseStatus
                    {
                        message = "Promotion is greater than Current Price.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if(addPromotion.startDate != null && addPromotion.endDate != null)
                {
                    if(addPromotion.startDate >= addPromotion.endDate)
                    {
                        return new ResponseStatus
                        {
                            message = "Promotion end date is less than start date.",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                    }
                }
                #endregion
                Promotion promotion = new Promotion();
                promotion.productId = addPromotion.productId;
                promotion.fixedAmount = addPromotion.fixedAmount;
                promotion.percent = addPromotion.percent;
                promotion.createdDate = addPromotion.createdDate;
                promotion.createdBy = addPromotion.createdBy;
                promotion.startDate = addPromotion.startDate;
                promotion.endDate = addPromotion.endDate;
                promotion.timeLimited = addPromotion.timeLimited;
                _context.Promotions.Add(promotion);
                await _context.SaveChangesAsync();

                throw new InvalidOperationException("This is a runtime exception.");
                return new ResponseStatus
                {
                    message = "Promotion Created Successfully",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public async Task<ResponseStatus> RemovePromotion(int id)
        {
            try
            {
                if(id == 0)
                {
                    return new ResponseStatus
                    {
                        message = "Invalid Id",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                var promotion = await _context.Promotions.Where(p => p.id == id).FirstOrDefaultAsync();
                if(promotion == null)
                {
                    return new ResponseStatus
                    {
                        message = "Promotion not found",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
                return new ResponseStatus
                {
                    message = "Promotion Removed Successfully",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch(Exception ex)
            {
                return new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public async Task<ResponseStatus> UpdatePromotion(UpdatePromotionRequest updatePromotion)
        {
            try
            {
                #region Validation
                if (updatePromotion == null)
                {
                    return new ResponseStatus
                    {
                        message = "Empty input for updating promotion",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                var product = await _context.Products.Where(p => p.Id == updatePromotion.productId).AsNoTracking().FirstOrDefaultAsync();
                if (product == null)
                {
                    return new ResponseStatus
                    {
                        message = "Product not found.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (updatePromotion.fixedAmount < 0 || updatePromotion.percent < 0 || updatePromotion.percent > 100)
                {
                    return new ResponseStatus
                    {
                        message = "Invalid promotion value.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (updatePromotion.fixedAmount > 0 && updatePromotion.percent > 0)
                {
                    return new ResponseStatus
                    {
                        message = "Both promotion types cannot be set at the same time.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (product.price_per_item < updatePromotion.fixedAmount)
                {
                    return new ResponseStatus
                    {
                        message = "Promotion is greater than Current Price.",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (updatePromotion.startDate != null && updatePromotion.endDate != null)
                {
                    if (updatePromotion.startDate >= updatePromotion.endDate)
                    {
                        return new ResponseStatus
                        {
                            message = "Promotion end date is less than start date.",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                    }
                }
                #endregion
                Promotion promotion;
                if (updatePromotion.productId != 0)
                {
                    promotion = await _context.Promotions.Where(p => p.productId == updatePromotion.productId).FirstOrDefaultAsync();
                }
                else
                {
                    promotion = await _context.Promotions.Where(p => p.id == updatePromotion.Id).FirstOrDefaultAsync();
                }
                promotion.fixedAmount = updatePromotion.fixedAmount;
                promotion.percent = updatePromotion.percent;
                promotion.updatedDate = updatePromotion.updatedDate;
                promotion.updatedBy = updatePromotion.updatedBy;
                promotion.startDate = updatePromotion.startDate;
                promotion.endDate = updatePromotion.endDate;
                promotion.timeLimited = updatePromotion.timeLimited;
                await _context.SaveChangesAsync();
                return new ResponseStatus
                {
                    message = "Promotion Updated Successfully",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}

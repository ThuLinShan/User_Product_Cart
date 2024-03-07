using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Cart;
using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.Promotion;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Helpers;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Repository
{
    public class CartRepository : ICart
    {
        CartResponse cartResponse = new CartResponse();
        private readonly DataContext _context;
        private IMapper _mapper = CartMapperConfig.InitializeAutomapper();

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ResponseStatus> AddCart(AddCartRequest addCart)
        {
            try
            {
                if (!_context.Users.Any(p => p.Id == addCart.UserId))
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "User Not Found";
                    return cartResponse;
                }
                var product = await _context.Products.Where(p => p.Id == addCart.ProductId).FirstOrDefaultAsync();
                if (product == null)
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "Product Not Found";
                    return cartResponse;
                }
                if (addCart.quantity <= 0)
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "Product quantity should be at least 1.";
                    return cartResponse;
                }
                var cart = await _context.Carts.Where(p => p.UserId == addCart.UserId && p.ProductId == addCart.ProductId).FirstOrDefaultAsync();
                if (cart == null)
                {
                    if (product.stock < addCart.quantity)
                    {
                        cartResponse.cartExists = false;
                        cartResponse.StatusCode = 400;
                        cartResponse.message = "Stock is less than quantity";
                        return cartResponse;
                    }
                    cart = new Cart();
                    cart.UserId = addCart.UserId;
                    cart.ProductId = addCart.ProductId;
                    cart.item_count = addCart.quantity;
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();

                    cartResponse.StatusCode = 200;
                    cartResponse.message = "Created New Cart.";
                    return cartResponse;
                }
                else
                {
                    if (product.stock < addCart.quantity + cart.item_count)
                    {
                        cartResponse.cartExists = false;
                        cartResponse.StatusCode = 400;
                        cartResponse.message = "Stock is less than quantity";
                        return cartResponse;
                    }
                    cart.item_count = cart.item_count + addCart.quantity;
                    //_context.Carts.Update(cart);
                    await _context.SaveChangesAsync();
                    cartResponse.StatusCode = 200;
                    cartResponse.message = "New product added to cart Successfully";
                    return cartResponse;
                }
            }
            catch (Exception ex)
            {
                cartResponse.StatusCode = 400;
                cartResponse.message = ex.Message;
                return cartResponse;
            }
        }

        public async Task<GetCartResponse> GetCart(int userId)
        {
            try
            {
                GetCartResponse getCart = new GetCartResponse();
                var cart = await _context.Carts
                        .Where(cart => cart.UserId == userId)
                        .Include(x=>x.Product)
                        .Select(cart => new GetCartProducts
                        {
                            id = cart.Product.Id,
                            product_name = cart.Product.product_name,
                            price_per_item = cart.Product.price_per_item,
                            quantity = cart.item_count,
                            promotionType = "",
                            promotionAmount = 0,
                            promotionPrice = 0,
                            difference = 0
                        })
                        .AsNoTracking()
                        .ToListAsync();
                if (cart == null)
                {
                    return getCart;
                }

                int totalPrice = 0;
                int totalPromotionPrice = 0;
                int itemPromotionPrice = 0;
                string _promotionType = "";
                int _promotionAmount = 0;
                int _difference = 0;
                List<Promotion> _promotions = await _context.Promotions.ToListAsync();

                #region Loop and calculate
                foreach (var result in cart)
                {
                    if (result.product_name== null) break;
                    var selectedPromotion = _promotions.Where(promotion => promotion.productId == result.id && ((promotion.timeLimited == true ? (promotion.startDate < DateTime.Now && promotion.endDate > DateTime.Now) : true))).FirstOrDefault();
                    itemPromotionPrice = 0;
                    _promotionType = "";
                    _promotionAmount = 0;
                    _difference = 0;
                    if (selectedPromotion != null)
                    {
                        itemPromotionPrice = selectedPromotion.fixedAmount > 0 ? result.price_per_item - selectedPromotion.fixedAmount :  result.price_per_item - (result.price_per_item * selectedPromotion.percent / 100);
                        _promotionType = selectedPromotion.fixedAmount > 0 ? "FixedAmount" : selectedPromotion.percent > 0 ? "Percent" : "";
                        _promotionAmount = selectedPromotion.fixedAmount > 0 ? selectedPromotion.fixedAmount : selectedPromotion.percent ;
                        _difference = selectedPromotion.fixedAmount > 0 ? selectedPromotion.fixedAmount : result.price_per_item * selectedPromotion.percent / 100 ;
                    }
                    result.promotionType = _promotionType;
                    result.promotionPrice = itemPromotionPrice;
                    result.promotionAmount = _promotionAmount;
                    result.difference = _difference;

                    totalPromotionPrice = totalPromotionPrice + (_difference * result.quantity);
                    totalPrice = totalPrice + (result.price_per_item * result.quantity);
                }
                #endregion

                getCart.cartProducts = cart;
                getCart.defaultPrice = totalPrice;
                getCart.userId = userId;
                getCart.userName = await _context.Users.Where(u => u.Id == userId).Select(u => u.firstname + u.lastname).FirstOrDefaultAsync();
                getCart.StatusCode = 200;
                getCart.message = "Success";
                getCart.totalPromotion = totalPromotionPrice;
                getCart.finalPrice = totalPrice - totalPromotionPrice;
                return getCart;
            }
            catch (Exception ex)
            {
                cartResponse.StatusCode = 400;
                cartResponse.message = ex.Message;
                return new GetCartResponse() { StatusCode = 400, message=ex.Message};
            }
        }

        public async Task<CartResponse> GetCarts()
        {
            try
            {
                cartResponse._cartDtos = _mapper.Map<List<CartDto>>(_context.Carts.OrderBy(p => p.UserId).ToList());
                cartResponse.StatusCode = 200;
                cartResponse.message = "Success";
                return cartResponse;
            }
            catch (Exception ex)
            {
                cartResponse.StatusCode = 500;
                cartResponse.message = ex.Message;
                return cartResponse;
            }
        }

        public async Task<ResponseStatus> RemoveProduct(int userId, int productId)
        {
            try
            {
                if (!_context.Users.Any(p => p.Id == userId))
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "User Not Found";
                    return cartResponse;
                }
                else if (!_context.Products.Any(p => p.Id == productId))
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "Product Not Found";
                    return cartResponse;
                }
                else if (!_context.Carts.Any(p => p.UserId == userId && p.ProductId == productId))
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "Cart Not Found";
                    return cartResponse;
                }
                else
                {
                    Cart cart = _context.Carts.Where(p => p.UserId == userId && p.ProductId == productId).FirstOrDefault();
                    _context.Carts.Remove(cart);
                    _context.SaveChanges();

                    cartResponse.StatusCode = 200;
                    cartResponse.message = "Product Removed Successfully";
                    return cartResponse;
                }
            }
            catch (Exception ex)
            {
                cartResponse.StatusCode = 400;
                cartResponse.message = "Failed";
                return cartResponse;
            }
        }

        public async Task<ResponseStatus> ReduceQuantity(int userId, int productId)
        {
            try
            {
                if (!_context.Users.Any(p => p.Id == userId))
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "User Not Found";
                    return cartResponse;
                }
                var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
                if (product ==null)
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "Product Not Found";
                    return cartResponse;
                }

                Cart cart =  _context.Carts.Where(p => p.UserId == userId && p.ProductId == productId).FirstOrDefault();
                if (cart == null)
                {
                    cartResponse.cartExists = false;
                    cartResponse.StatusCode = 400;
                    cartResponse.message = "Cart Not Found";
                    return cartResponse;
                }
                if(cart.item_count > 1)
                {
                    cart.item_count = cart.item_count - 1;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                    cartResponse.StatusCode = 200;
                    cartResponse.message = "Product removed by 1";
                    return cartResponse;
                }
                else
                {
                    _context.Carts.Remove(cart);
                    _context.SaveChanges();

                    cartResponse.StatusCode = 200;
                    cartResponse.message = "Success";
                    return cartResponse;
                }
            }
            catch (Exception ex)
            {
                cartResponse.StatusCode = 400;
                cartResponse.message = ex.Message;
                return cartResponse;
            }
        }
    }
}

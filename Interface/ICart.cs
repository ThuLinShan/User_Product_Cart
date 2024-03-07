using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.Cart;
using User_Product_Cart.Models;
using User_Product_Cart.Dtos;

namespace User_Product_Cart.Interface
{
    public interface ICart
    {
        Task<CartResponse> GetCarts();
        Task<GetCartResponse> GetCart(int userId);
        Task<ResponseStatus> AddCart(AddCartRequest addCartRequest);
        Task<ResponseStatus> ReduceQuantity(int userId, int productId);
        Task<ResponseStatus> RemoveProduct(int userId, int productId);
    }
}
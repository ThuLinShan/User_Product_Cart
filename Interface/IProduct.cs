using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.Product;
using User_Product_Cart.Models;

namespace User_Product_Cart.Interface
{
    public interface IProduct
    {
        Task<ProductResponse> GetProducts();
        Task<ProductResponse> GetProductNames();
        Task<ProductResponse> GetProduct(int id);
        Task<ProductResponse> GetRecentProducts();
        Task<ProductResponse> ProductExists(int productId);
        Task<ResponseStatus> CreateProduct(AddProductRequest addProduct);
        Task<ResponseStatus> UpdateProduct(UpdateProductRequest productDto, int productId);
        Task<ResponseStatus> DeleteProduct(int productId);
    }
}

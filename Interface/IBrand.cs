using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Brand;

namespace User_Product_Cart.Interface
{
    public interface IBrand
    {
        Task<BrandResponse> GetBrandById(int id);
        Task<GetBrandListResponse> GetBrandList();
        Task<ResponseStatus> AddProductToBrand(int productId, int brandId);
        Task<ResponseStatus> CreateBrand(CreateBrandRequest req);
        Task<ResponseStatus> UpdateBrand(UpdateBrandRequest req);
        Task<ResponseStatus> DeleteBrand(int id);
    }
}

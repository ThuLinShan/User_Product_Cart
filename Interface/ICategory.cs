using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Category;

namespace User_Product_Cart.Interface
{
    public interface ICategory
    {
        Task<MainCategoryListResponse> GetMainCategory();
        Task<CategoryResponse> GetCategoryById(int id);
        Task<GetCategoryListResponse> GetCategoryList();
        Task<GetMainCategoryProducts> GetCategorizedProducts();
        Task<ResponseStatus> AddProductToCategory(int categoryId, int productId);
        Task<ResponseStatus> CreateCategory(CreateCategoryRequest req);
        Task<ResponseStatus> UpdateCategory(int id, UpdateCategoryRequest req);
        Task<ResponseStatus> DeleteCategory(int id);
    }
}

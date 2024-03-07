using User_Product_Cart.Dtos.Product;
namespace User_Product_Cart.Dtos.Category
{
    public class GetMainCategoryProducts: ResponseStatus
    {
        public List<MainCategoryList> MainCategories { get; set; }
    }
    public class MainCategoryList
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryProductItem> CategoryProducts { get; set; }
    }
    public class CategoryProductItem
    {
        public int productId { get; set; }
        public string productName {  get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public int? parentCategoryId { get; set; }
        public string? parentCategoryName { get; set; }
        public DateTime created_date { get; set; }
        public int stock { get; set; }
    }
}

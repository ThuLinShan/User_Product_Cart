using User_Product_Cart.Dtos.Product;

namespace User_Product_Cart.Dtos.Category
{
    public class MainCategoryListResponse: ResponseStatus
    {
        public List<MainCategories> mainCategories {  get; set; }
    }
    public class MainCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChildCategories> childCategories { get; set; }
    }
    public class ChildCategories    
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? parentId { get; set; } = null;
        public List<ProductDto> products  { get; set; }
    }
}

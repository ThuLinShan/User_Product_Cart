namespace User_Product_Cart.Dtos.Category
{
    public class CategoryResponse: ResponseStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? parentId { get; set; } = null;
    }
}

namespace User_Product_Cart.Dtos.Category
{
    public class GetCategoryListResponse: ResponseStatus
    {
        public int ItemCount { get; set; }
        public List<CategoryResponse> Categories { get; set; }
    }
}

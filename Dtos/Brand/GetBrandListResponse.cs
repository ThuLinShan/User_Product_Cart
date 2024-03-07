namespace User_Product_Cart.Dtos.Brand
{
    public class GetBrandListResponse : ResponseStatus
    {
        public List<BrandListItem> brands { get; set; }
    }

    public class BrandListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

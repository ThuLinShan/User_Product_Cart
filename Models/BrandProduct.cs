namespace User_Product_Cart.Models
{
    public class BrandProduct
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Brand Brand { get; set; }
    }
}

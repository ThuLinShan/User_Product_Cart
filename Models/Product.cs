namespace User_Product_Cart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string product_name { get; set; }
        public int price_per_item { get; set; }
        public string category { get; set; }
        public DateTime created_date { get; set; }
        public int stock {  get; set; } = 0;
        public virtual ICollection<EventProduct> EventProducts { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
        public virtual ICollection<BrandProduct> BrandProducts { get; set; }
    }
}

using User_Product_Cart.Dtos.Product;
using User_Product_Cart.Dtos.Promotion;

namespace User_Product_Cart.Dtos.Product
{
    public class ProductDto
    {
        public string product_name { get; set; }
        public int price_per_item { get; set; }
        public string category { get; set; }
        public DateTime created_date { get; set; }
        public int stock { get; set; }
        public ProductPromotionResponse promotion { get; set; }
    }
}
using User_Product_Cart.Dtos.Promotion;

namespace User_Product_Cart.Dtos.Product
{
    public class AddProductRequest
    {
        public string product_name { get; set; }
        public int price_per_item { get; set; }
        public string category { get; set; }
        public DateTime created_date { get; set; }
        public int stock { get; set; }
        public AddPromotionWithProduct? addPromotion { get; set; } = null;
    }
}

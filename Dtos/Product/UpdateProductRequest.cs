using User_Product_Cart.Dtos.Promotion;

namespace User_Product_Cart.Dtos.Product
{
    public class UpdateProductRequest
    {
        public string product_name { get; set; }
        public int price_per_item { get; set; }
        public string category { get; set; }
        public int stock { get; set; }
        public UpdatePromotionWtihProduct? updatePromotion{ get; set; } = null;
    }
}

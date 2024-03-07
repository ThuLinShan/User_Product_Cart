namespace User_Product_Cart.Dtos.Cart
{
    public class GetCartResponse : ResponseStatus
    {
        public List<GetCartProducts> cartProducts {  get; set; }
        public int userId { get; set; }
        public int totalPromotion { get; set; }
        public int defaultPrice { get; set; }
        public int finalPrice { get; set; }
        public string userName { get; set; }
    }
    public class GetCartProducts
    {
        public int id { get; set; }
        public string product_name { get; set; }
        public int price_per_item { get; set; }
        public int quantity { get; set; }
        public string? promotionType { get; set; }
        public int? promotionAmount { get; set; }
        public int? promotionPrice { get; set; }
        public int? difference { get; set; }
    }
}

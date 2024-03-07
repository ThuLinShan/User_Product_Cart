namespace User_Product_Cart.Dtos.Cart
{
    public class AddCartRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int quantity { get; set; }
    }
}

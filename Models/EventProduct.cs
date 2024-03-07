namespace User_Product_Cart.Models
{
    public class EventProduct
    {
        public int ProductId { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public virtual Product Product { get; set; }
    }
}

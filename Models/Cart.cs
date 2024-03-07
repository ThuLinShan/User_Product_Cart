namespace User_Product_Cart.Models
{
    public class Cart
    {
        public int UserId{ get; set; }
        public virtual User User{ get; set; }
        public int ProductId{ get; set; }
        public virtual Product Product {  get; set; }
        public int item_count { get; set; } = 1;
    }
}

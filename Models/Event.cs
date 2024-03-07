namespace User_Product_Cart.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int percent { get; set; }
        public virtual ICollection<EventProduct> EventProducts { get; set; }
    }
}

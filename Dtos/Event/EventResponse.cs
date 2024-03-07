namespace User_Product_Cart.Dtos.Event
{
    public class EventResponse: ResponseStatus
    {
        public string Name { get; set; }
        public int percent { get; set; }
        public List<EventProducts> EventProducts { get; set; }
    }

    public class EventProducts
    {
        public string ProductName { get; set; }
        public int DefaultPrice { get; set; }
        public int PromotionPrice { get; set; }
        public string category { get; set; }
        public DateTime created_date { get; set; }
        public int stock { get; set; }
    }
}

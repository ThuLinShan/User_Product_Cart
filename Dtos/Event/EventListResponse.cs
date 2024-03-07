namespace User_Product_Cart.Dtos.Event
{
    public class EventListResponse:ResponseStatus
    {
        public List<EventItem> EventItems { get; set; }
    }
    public class EventItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int percent { get; set; }
    }
}

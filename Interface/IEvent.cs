using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Event;

namespace User_Product_Cart.Interface
{
    public interface IEvent
    {
        Task<EventResponse> GetEvent(int id);
        Task<EventListResponse> GetEventList();
        Task<ResponseStatus> AddEvent(AddEventRequest addEvent);
        Task<ResponseStatus> AddProductToEvent(int eventId, int productId);
        Task<ResponseStatus> UpdateEvent(UpdateEventRequest updateEvent);
        Task<ResponseStatus> RemoveEvent(int id);
    }
}

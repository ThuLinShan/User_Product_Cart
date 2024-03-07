using Microsoft.AspNetCore.Mvc;
using User_Product_Cart.Dtos.Event;
using User_Product_Cart.Interface;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEvent _eventRepository;
        public EventController(IEvent eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet("GetEvent")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var response = await _eventRepository.GetEvent(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("EventList")]
        public async Task<IActionResult> GetEvents()
        {
            var response = await _eventRepository.GetEventList();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(AddEventRequest addEvent)
        {
            var response = await _eventRepository.AddEvent(addEvent);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Add Product to Event")]
        public async Task<IActionResult> AddProductToEvent(int eventId, int productId)
        {
            var response = await _eventRepository.AddProductToEvent(eventId, productId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(UpdateEventRequest updateEvent)
        {
            var respone = await _eventRepository.UpdateEvent(updateEvent);
            return StatusCode(respone.StatusCode, respone);
        }
        [HttpDelete("DeleteEvent")]
        public async Task<IActionResult> RemoveEvent(int id)
        {
            var respone = await _eventRepository.RemoveEvent(id);
            return StatusCode(respone.StatusCode, respone);
        }
    }
}

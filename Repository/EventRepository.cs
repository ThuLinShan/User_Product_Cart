using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos;
using User_Product_Cart.Dtos.Event;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Repository
{
    public class EventRepository : IEvent
    {
        private readonly DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ResponseStatus> AddEvent(AddEventRequest addEvent)
        {
            try
            {
                Event _event = new Event();
                _event.Name = addEvent.Name;
                _event.percent = addEvent.percent;
                await _context.Events.AddAsync(_event);
                await _context.SaveChangesAsync();

                return (new ResponseStatus
                {
                    message = "New Event Added Successfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch(Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        public async Task<ResponseStatus> AddProductToEvent(int eventId, int productId)
        {
            try
            {
                if(_context.Products.Where(p => p.Id == productId).Any()) { 
                    if(_context.Events.Where(e => e.Id == eventId).Any())
                    {
                        await _context.EventsProduct.AddAsync(new EventProduct { EventId = eventId, ProductId = productId });
                        await _context.SaveChangesAsync();
                        return (new ResponseStatus
                        {
                            message = "Product is added to event successfully.",
                            StatusCode = StatusCodes.Status200OK
                        });
                    }
                    return (new ResponseStatus
                    {
                        message = "Event not found",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }
                return (new ResponseStatus
                {
                    message = "Product not found",
                    StatusCode = StatusCodes.Status400BadRequest
                }) ;
            }
            catch(Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        public async Task<EventResponse> GetEvent(int id)
        {
            try 
            {
                EventResponse eventResponse = new EventResponse();
                Event _event = await _context.Events.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (_event == null)
                {
                    eventResponse.message = "Event not found";
                    eventResponse.StatusCode = StatusCodes.Status400BadRequest;
                    return eventResponse;
                }
                eventResponse.Name = _event.Name;
                eventResponse.percent = _event.percent;
                eventResponse.EventProducts = await _context.EventsProduct.Where(ep => ep.EventId == id)
                                                                           .Select(ep => new EventProducts
                                                                           {
                                                                               ProductName = ep.Product.product_name,
                                                                               DefaultPrice= ep.Product.price_per_item,
                                                                               PromotionPrice = ep.Product.price_per_item - (ep.Product.price_per_item * _event.percent / 100),
                                                                               category = ep.Product.category,
                                                                               created_date = ep.Product.created_date,
                                                                               stock = ep.Product.stock
                                                                           }).ToListAsync();

                eventResponse.message = "Get Event by Id success.";
                eventResponse.StatusCode = StatusCodes.Status200OK;
                return eventResponse;
            }
            catch(Exception ex)
            {
                return (new EventResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        public async Task<EventListResponse> GetEventList()
        {
            try
            {
                return (new EventListResponse
                {
                    EventItems = await _context.Events.Select(e => new EventItem
                    {
                        id = e.Id,
                        name = e.Name,
                        percent = e.percent
                    }).ToListAsync(),
                    StatusCode = StatusCodes.Status200OK,
                    message = "Get Event List Successful."
                });
            }
            catch (Exception ex)
            {
                return (new EventListResponse
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        public async Task<ResponseStatus> RemoveEvent(int id)
        {
            try
            {
                var dbEvent = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();
                if (dbEvent == null)
                {
                    return (new ResponseStatus
                    {
                        message = "Event Not Found",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }
                _context.Events.Remove(dbEvent);
                return (new ResponseStatus
                {
                    message = "Event Removed Succcessfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        public async Task<ResponseStatus> UpdateEvent(UpdateEventRequest updateEvent)
        {
            try
            {
                var dbEvent = await _context.Events.Where(e => e.Id == updateEvent.Id).FirstOrDefaultAsync();
                if(dbEvent == null)
                {
                    return (new ResponseStatus
                    {
                        message = "Event Not Found",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }
                dbEvent.Name = updateEvent.Name;
                dbEvent.percent = updateEvent.percent;
                await _context.SaveChangesAsync();
                return (new ResponseStatus
                {
                    message = "Event Updated Succcessfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return (new ResponseStatus
                {
                    message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}

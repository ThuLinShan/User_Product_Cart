using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using User_Product_Cart.Dtos.Cart;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Helpers;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICart _cartRepository;
        //Constructor
        public CartController(ICart cartRepository)
        {
            _cartRepository = cartRepository;
        }


        //Create User
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddCart([FromBody] AddCartRequest addCartDto)
        {
            var response = await _cartRepository.AddCart(addCartDto);
            if (response.StatusCode == 400)
            {
                ModelState.AddModelError("",  response.message);
                return StatusCode(400, ModelState);
            }
            else
            {
                return Ok(response.message);
            }
        }

        [HttpGet] //Get all
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartDto>))]
        public async Task<IActionResult> GetCarts()
        {
            try
            {
                var response = await _cartRepository.GetCarts();
                return Ok(response._cartDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")] //Get user by Id
        [ProducesResponseType(200, Type = typeof(CartDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                var cartResponse = await _cartRepository.GetCart(userId);

                return Ok(cartResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Create User
        [HttpDelete("ReduceQuantity")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReduceQuantity([FromBody] UpdateCartDto addCartDto)
        {
            var response = await _cartRepository.ReduceQuantity(addCartDto.UserId, addCartDto.ProductId);

            if (response.StatusCode == 400)
            {
                ModelState.AddModelError("", response.message);
                return StatusCode(400, ModelState);
            }
            else
            {
                return Ok(response.message);
            }
        }

        //Create User
        [HttpDelete("RemoveProduct")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveProduct([FromBody] UpdateCartDto addCartDto)
        {
            var response = await _cartRepository.RemoveProduct(addCartDto.UserId, addCartDto.ProductId);
            if (response.StatusCode == 400)
            {
                ModelState.AddModelError("", "Something went wrong while removing product from cart");
                return StatusCode(400, ModelState);
            }
            else
            {
                return Ok(response.message);
            }
        }
    }
}

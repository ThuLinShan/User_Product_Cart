using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Helpers;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _userRepository;
        //Constructor
        public UserController(IUser userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet] //Get all
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var response = await _userRepository.GetUsers();
                return Ok(response._userDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")] //Get user by Id
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _userRepository.GetUser(userId);

                return Ok(user._userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Create User
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUser)
        {
            if (createUser == null)
            {
                return BadRequest("User is empty");
            }

            var response = await _userRepository.CreateUser(createUser);

            if (response.StatusCode == 500)
            {
                ModelState.AddModelError("", "Something went wrong while creating new user");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok("User created successfully.");
            }
        }


        //Update User
        [HttpPut("userId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUser, int userId)
        {
            if (updateUser == null)
            {
                return BadRequest(ModelState);
            }

            var response = await _userRepository.UserExists(userId);
            if (!response.userExists)
            {
                return NotFound("User not found");
            }
            response = await _userRepository.UpdateUser(updateUser , userId);
            if (response.StatusCode == 500)
            {
                ModelState.AddModelError("", "Something went wrong while updating new user" + response.message);
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok("User updated successfully.");
            }
        }

        //Delete User
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var response = await _userRepository.UserExists(userId);
            if (!response.userExists)
            {
                return NotFound("User not found");
            }
            response = await _userRepository.DeleteUser(userId);
            if (response.StatusCode == 400)
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok("User deleted successfully");
            }
        }
    }
}

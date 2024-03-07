using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Net.NetworkInformation;
using User_Product_Cart.Context;
using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Helpers;
using User_Product_Cart.Interface;
using User_Product_Cart.Models;

namespace User_Product_Cart.Repository
{
    public class UserRepository : IUser
    {
        UserResponse userResponse = new UserResponse();
        private readonly DataContext _context;
        private IMapper _mapper = UserMapperConfig.InitializeAutomapper();
        private readonly IMemoryCache _cache;
        private readonly string cacheKey = "userCacheKey";

        public UserRepository(DataContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }


        //Get by Id----------
        public async Task<UserResponse> GetUser(int id)
        {
            try
            {
                User dbUser = _context.Users.Where(p => p.Id == id && p.isActive == true).FirstOrDefault();
                if (dbUser == null)
                {
                    userResponse.StatusCode = 500;
                    userResponse.message = "User Not found";
                    return userResponse;
                }

                UserDto userDto = new UserDto();
                userDto.firstname = dbUser.firstname;
                userDto.lastname = dbUser.lastname;
                userDto.email = dbUser.email;
                userDto.created_date = dbUser.created_date;
                userDto.createdBy = dbUser.createdBy;
                userDto.updated_date = dbUser.updated_date;
                userDto.updatedBy = dbUser.updatedBy;
                userResponse._userDto = userDto;
                userResponse.StatusCode = 200;
                userResponse.message = "Success";
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.StatusCode = 400;
                userResponse.message = ex.Message;
                return userResponse;
            }
        }

        //Get users ---------
        public async Task<UserResponse> GetUsers()
        {
            try
            {
                //To check if there are values in cache
                if (_cache.TryGetValue(cacheKey, out IEnumerable<User> users))
                {
                    Console.WriteLine("\nUsers Cache Found");
                }
                //If cache value is not available, get from database and create cache
                else
                {
                    Console.WriteLine("\nUsers Cache Not Found");
                    users = _context.Users.OrderBy(p => p.Id).Where(p => p.isActive == true).ToList();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromHours(3600))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(cacheKey, users, cacheEntryOptions);
                }
                userResponse._userDtos = _mapper.Map<List<UserDto>>(users);
                userResponse.StatusCode = 200;
                userResponse.message = "Success";
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.StatusCode = 500;
                userResponse.message = ex.Message;
                return userResponse;
            }
        }
        //Create-------------
        public async Task<UserResponse> CreateUser(CreateUserDto userDto)
        {
            try
            {
                User user = new User();
                user.firstname = userDto.firstname;
                user.lastname = userDto.lastname;
                user.email = userDto.email;
                user.created_date = userDto.created_date;
                user.createdBy = userDto.createdBy;
                _context.Users.Add(user);
                _context.SaveChanges();

                userResponse.StatusCode = 200;
                userResponse.message = "Success";
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.StatusCode = 500;
                userResponse.message = "Failed";
                return userResponse;
            }
        }
        
        //Delete-------------
        public async Task<UserResponse> DeleteUser(int userId)
        {
            try
            {
                User user = _context.Users.Where(p => p.Id == userId && p.isActive == true).FirstOrDefault();
                user.Id = userId;
                user.isActive = false;
                _context.Users.Update(user);
                _context.SaveChanges();

                userResponse.StatusCode = 200;
                userResponse.message = "Success";
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.StatusCode = 400;
                userResponse.message = "Failed";
                return userResponse;
            }
        }


        public async Task<UserResponse> UpdateUser(UpdateUserDto userDto, int userId)
        {
            try
            {
                User user = _context.Users.Where(p => p.Id == userId && p.isActive == true).FirstOrDefault();
                user.Id = userId;
                user.firstname = userDto.firstname;
                user.lastname = userDto.lastname;
                user.email = userDto.email;
                user.updated_date = userDto.updated_date;
                user.updatedBy = userDto.updatedBy;
                _context.Users.Update(user);
                _context.SaveChanges();
                userResponse.StatusCode = 200;
                userResponse.message = "User Updated Successfully";
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.StatusCode = 500;
                userResponse.message = ex.Message;
                return userResponse;
            }
        }

        public async Task<UserResponse> UserExists(int userId)
        {
            userResponse.userExists = false;
            if (_context.Users.Any(p => p.Id == userId && p.isActive == true))
            {
                userResponse.userExists = true;
            }
            return userResponse;
        }
    }
}

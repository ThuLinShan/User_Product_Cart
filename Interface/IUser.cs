using User_Product_Cart.Dtos.Helper;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Models;

namespace User_Product_Cart.Interface
{
    public interface IUser
    {
        Task<UserResponse> GetUsers();
        Task<UserResponse> GetUser(int id);
        Task<UserResponse> UserExists(int userId);
        Task<UserResponse> CreateUser(CreateUserDto userDto);
        Task<UserResponse> UpdateUser(UpdateUserDto userDto, int userId);
        Task<UserResponse> DeleteUser(int userId);
    }
}

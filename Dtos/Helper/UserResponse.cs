using User_Product_Cart.Dtos.User;

namespace User_Product_Cart.Dtos.Helper
{
    public class UserResponse : ResponseStatus
    {
        public string message;
        public UserDto _userDto;
        public List<UserDto> _userDtos;
        public bool userExists;

        public UserDto GetUsertDto(UserDto userDto)
        {
            _userDto = userDto;
            return _userDto;
        }
        public List<UserDto> GetUsertDtos(List<UserDto> userDtos)
        {
            _userDtos = userDtos;
            return _userDtos;
        }
    }
}

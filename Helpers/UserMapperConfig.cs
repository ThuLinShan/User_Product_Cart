using AutoMapper;
using User_Product_Cart.Dtos.User;
using User_Product_Cart.Models;

namespace User_Product_Cart.Helpers
{
    public class UserMapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}

using AutoMapper;
using User_Product_Cart.Dtos.Cart;
using User_Product_Cart.Models;

namespace User_Product_Cart.Helpers
{
    public class CartMapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CartDto>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}

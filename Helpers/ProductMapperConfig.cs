using AutoMapper;
using User_Product_Cart.Dtos.Product;
using User_Product_Cart.Models;

namespace User_Product_Cart.Helpers
{
    public class ProductMapperConfig
    {

        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<Product, ProductNameDto>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}

using User_Product_Cart.Dtos.Product;

namespace User_Product_Cart.Dtos.Helper
{
    public class ProductResponse : ResponseStatus
    {
        public ProductDto _productDto;
        public ProductNameDto _productNameDto;
        public List<ProductDto> _productDtos;
        public List<ProductNameDto> _productNameDtos;
        public bool productExists;

        public ProductDto GetProducttDto(ProductDto productDto)
        {
            _productDto = productDto;
            return _productDto;
        }
        public List<ProductDto> GetProducttDtos(List<ProductDto> productDtos)
        {
            _productDtos = productDtos;
            return _productDtos;
        }
    }
}

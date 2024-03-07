using User_Product_Cart.Dtos.Cart;

namespace User_Product_Cart.Dtos.Helper
{
    public class CartResponse : ResponseStatus
    {
        public CartDto _cartDto;
        public List<CartDto> _cartDtos;
        public bool cartExists;

        public CartDto GetCarttDto(CartDto cartDto)
        {
            _cartDto = cartDto;
            return _cartDto;
        }
        public List<CartDto> GetCarttDtos(List<CartDto> cartDtos)
        {
            _cartDtos = cartDtos;
            return _cartDtos;
        }
    }
}

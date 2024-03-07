using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Dtos.Brand
{
    public class BrandResponse: ResponseStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

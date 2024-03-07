using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; } = true;
        public virtual ICollection<BrandProduct> BrandProducts { get; set; }
    }
}

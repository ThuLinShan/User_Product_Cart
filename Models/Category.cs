using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; } = null;

        public string Name { get; set; }
        public bool isActive { get; set; } = true;
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}

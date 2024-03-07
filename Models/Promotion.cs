using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Models
{
    public class Promotion
    {
        public int id { get; set; }
        public int productId { get; set; }
        public virtual Product Product { get; set; }
        public int fixedAmount { get; set; } = 0;
        public int percent { get; set; } = 0;
        public DateTime createdDate { get; set; } = DateTime.Now;
        public string createdBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public string? updatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; }
        public bool timeLimited { get; set; } = false;
    }
}

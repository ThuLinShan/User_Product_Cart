using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Dtos.Promotion
{
    public class AddPromotionRequest
    {
        public int productId { get; set; }
        public int fixedAmount { get; set; } = 0;
        public int percent { get; set; } = 0;
        public DateTime createdDate { get; set; } = DateTime.Now;
        public string createdBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; } = null;
        public bool timeLimited { get; set; } = false;
    }
}

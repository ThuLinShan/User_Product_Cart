using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Dtos.Promotion
{
    public class UpdatePromotionRequest
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public int fixedAmount { get; set; } = 0;
        public int percent { get; set; } = 0;
        public DateTime updatedDate { get; set; }
        public string updatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; } = null;
        public bool timeLimited { get; set; } = false;
    }
}

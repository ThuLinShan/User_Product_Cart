using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Dtos.Promotion
{
    public class PromotionResponse: ResponseStatus
    {
        public int id { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public int fixedAmount { get; set; } = 0;
        public int percent { get; set; } = 0;
        public int defaultPrice { get; set; }
        public int promotionPrice { get; set; }
        public int differencePrice { get; set; }
        public DateTime createdDate { get; set; } = DateTime.Now;
        public string createdBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public string? updatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; } = null;
        public bool timeLimited { get; set; } = false;
    }
}

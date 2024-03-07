using System.ComponentModel.DataAnnotations;

namespace User_Product_Cart.Dtos.Promotion
{
    public class UpdatePromotionWtihProduct
    {
        public int fixedAmount { get; set; } = 0;
        public int percent { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public string By { get; set; }
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; } = null;
        public bool timeLimited { get; set; } = false;
    }
}

namespace User_Product_Cart.Models
{
    public class User
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public DateTime created_date { get; set; }
        public string createdBy { get; set; }
        public DateTime? updated_date { get; set; }
        public string? updatedBy { get; set; }
        public bool isActive { get; set; } = true;
    }
}

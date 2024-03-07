namespace User_Product_Cart.Dtos.User
{
    public class UpdateUserDto
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public DateTime? updated_date { get; set; }
        public string? updatedBy { get; set; }
    }
}

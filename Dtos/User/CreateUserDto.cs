namespace User_Product_Cart.Dtos.User
{
    public class CreateUserDto
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public DateTime created_date { get; set; }
        public string createdBy { get; set; }
    }
}

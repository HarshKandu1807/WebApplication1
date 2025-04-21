namespace WebApplication1.Model.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string ContactNo { get; set; }
        //public string Password { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}

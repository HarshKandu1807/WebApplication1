using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be exactly 10 digits.")]
        public string ContactNo { get; set; }
        //public string Password { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}

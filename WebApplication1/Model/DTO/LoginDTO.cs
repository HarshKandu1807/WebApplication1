using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model.DTO
{
    public class LoginDTO
    {
        public string ContactNo { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be exactly 10 digits.")]
        public string Password { get; set; }
    }
}

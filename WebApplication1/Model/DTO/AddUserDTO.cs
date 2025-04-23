using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model.DTO
{
    public class AddUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be exactly 10 digits.")]
        public string ContactNo { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

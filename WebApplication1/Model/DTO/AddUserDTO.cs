using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model.DTO
{
    public class AddUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Contact number cannot exceed 10 digits")]
        public string ContactNo { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

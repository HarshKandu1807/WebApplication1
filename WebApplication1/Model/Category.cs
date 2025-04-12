using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}

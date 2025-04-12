using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Model
{
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        //public bool Isdeleted { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}

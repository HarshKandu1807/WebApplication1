using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication1.Model;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int UserId { get; set; }
    
    [JsonIgnore]
    public Users User { get; set; }
    public int ProductId { get; set; }
    [JsonIgnore]
    public Products Product { get; set; }
    public int Quantity { get; set; }

    public DateTime OrderDate { get; set; }
}

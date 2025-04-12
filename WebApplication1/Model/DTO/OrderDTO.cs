namespace WebApplication1.Model.DTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }      // FK value
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

namespace WebApplication1.Model.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }//Foreign Key
    }
}

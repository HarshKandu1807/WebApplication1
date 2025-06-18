using WebApplication1.Model;
using WebApplication1.Model.DTO;

namespace WebApplication1.Services.Iservices
{
    public interface IProducts
    {
        Task<List<ProductDTO>?> GetAllProducts(int page = 1, int pageSize = 4);
        Task<ProductDTO?> GetProductById(int id);
        Task<ProductDTO?> AddProduct(ProductDTO productDTO);
        Task<ProductDTO?> UpdateProduct(int id,ProductDTO productDTO);
        Task<bool> DeleteProduct(int id);
    }
}

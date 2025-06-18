using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Services
{
    public class ProductServices:IProducts
    {
        private readonly UserContext _db;
        public ProductServices(UserContext _db)
        {
            this._db = _db;
        }
        public async Task<List<ProductDTO>?> GetAllProducts(int page=1,int pageSize=4)
        {
            var totalcount = _db.Products.Count();
            var totalpages = (int)Math.Ceiling((double)totalcount / pageSize);
            var data = await _db.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var products = await _db.Products.Select(x => new ProductDTO
            {
                Name = x.Name,
                Price=x.Price,
                CategoryId=x.CategoryId
            }).ToListAsync();
            if (products == null)
            {
                return null;
            }
            return products;

        }
        public async Task<ProductDTO?> GetProductById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            var result = new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            return result;
        }
        public async Task<ProductDTO?> AddProduct(ProductDTO productDTO)
        {
            var categoryId = await _db.Categories.AnyAsync(x => x.CategoryId == productDTO.CategoryId);
            if (!categoryId)
            {
                return null;
            }
            var result = new Products
            {
                Name = productDTO.Name,
                Price=productDTO.Price,
                CategoryId=productDTO.CategoryId
            };
            _db.Products.Add(result);
            await _db.SaveChangesAsync();
            return productDTO;
        }
        public async Task<ProductDTO?> UpdateProduct(int id, ProductDTO productDTO)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.CategoryId = productDTO.CategoryId;
                await _db.SaveChangesAsync();
                return productDTO;
            }
            return null;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

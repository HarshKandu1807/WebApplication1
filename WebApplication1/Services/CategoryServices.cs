using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Services
{
    public class CategoryServices:ICategory
    {
        private readonly UserContext _db;
        public CategoryServices(UserContext _db)
        {
            this._db = _db;
        }
        public async Task<List<CategoryDTO>?> GetAllCategory()
        {
            var categories = await _db.Categories.Select(x=>new CategoryDTO
            {
                Name=x.Name
            }).ToListAsync();
            if (!categories.Any())
            {
                return null;
            }
            return categories;
        }
        public async Task<CategoryDTO?> GetCategoryById(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if(category != null)
            {
                var result = new CategoryDTO
                {
                    Name = category.Name
                };
                return result;
            }
            return null;
        }
        public async Task<CategoryDTO?> AddCategory(CategoryDTO categoryDTO)
        {
            var category = await _db.Categories.AnyAsync(x => x.Name == categoryDTO.Name);
            if (!category)
            {
                var result = new Category
                {
                    Name = categoryDTO.Name,
                };
                _db.Categories.Add(result);
                await _db.SaveChangesAsync();
                return categoryDTO;
            }
            return null;
        }
        public async Task<CategoryDTO?> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var category = await _db.Categories.FindAsync(id);
            if(category != null)
            {
                category.Name = categoryDTO.Name;
                await _db.SaveChangesAsync();
                return categoryDTO;
            }
            return null;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

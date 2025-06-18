using WebApplication1.Model.DTO;

namespace WebApplication1.Services.Iservices
{
    public interface ICategory
    {
        Task<List<CategoryDTO>?> GetAllCategory();
        Task<CategoryDTO?> GetCategoryById(int id);
        Task<CategoryDTO?> AddCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO?> UpdateCategory(int id, CategoryDTO categoryDTO);
        Task<bool> DeleteCategory(int id);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly UserContext context;
        public CategoriesController(UserContext userContext)
        {
            context = userContext;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await context.Categories.ToListAsync();
            if (categories == null)
            {
                return NotFound("No Categories Exist");
            }
            return Ok(categories);
        }

        [HttpGet("GetCategory/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Category Not Found");
            }
            return Ok(category);
        }

        [HttpPost("AddCategories")]
        public async Task<IActionResult> AddCategories([FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = new Category
            {
                Name = categoryDto.Name
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Category Not Found");
            }
            category.Name = categoryDto.Name;
            await context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Category Not Found");
            }
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok("Category Deleted");
        }
    }
}

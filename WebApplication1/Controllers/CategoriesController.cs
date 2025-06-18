using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory Icategory;
        public CategoriesController(ICategory Icategory)
        {
            this.Icategory = Icategory;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var category = await Icategory.GetAllCategory();
            if (category == null)
            {
                return NotFound("Category Does Not Exist");
            }
            return Ok(category);
        }

        [HttpGet("GetCategory/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await Icategory.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Category Does Not Exist");
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
            var category = await Icategory.AddCategory(categoryDto);
            if (category == null)
            {
                return BadRequest("Category Already Exist");
            }
            return Ok(category);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var category = await Icategory.UpdateCategory(id,categoryDto);
            if (category == null)
            {
                return BadRequest();
            }
            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await Icategory.DeleteCategory(id);
            if (category == true)
            {
                return Ok("Category Deleted Successfully");
            }
            else
            {
                return NotFound("Category Does Not Exist");
            }
        }
    }
}

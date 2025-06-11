using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
//using WebApplication1.Migrations;
using WebApplication1.Model;
using WebApplication1.Model.DTO;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserContext ProductContext;
        public ProductController(UserContext ProductContext)
        {
            this.ProductContext = ProductContext;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<ActionResult> GetProducts(int page = 1,int pageSize=4)
        {
            var totalcount = ProductContext.Products.Count();
            var totalpages = (int)Math.Ceiling((double)totalcount / pageSize);
            var data = await ProductContext.Products.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpGet]
        [Route("GetProduct{id}")]
        public async Task<IActionResult> getProductByid(int id)
        {
            var data = await ProductContext.Products.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult> AddProducts(ProductDTO Productdto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new WebApplication1.Model.Products
            {
                Name = Productdto.Name,
                Price = Productdto.Price,
                CategoryId = Productdto.CategoryId
            };

            await ProductContext.Products.AddAsync(product); // FIX: Added await
            await ProductContext.SaveChangesAsync();

            return Ok(product);
        }



        [HttpPut]
        [Route("UpdateProduct{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDTO Products)
        {
            var product = await ProductContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }
            product.Name = Products.Name;
            product.Price = Products.Price;
            product.CategoryId = Products.CategoryId;
            ProductContext.Products.Update(product);
            await ProductContext.SaveChangesAsync();
            return Ok(product);
        }


        [HttpDelete]
        [Route("DeleteProduct{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var std = await ProductContext.Products.FindAsync(id);
            if (std == null)
            {
                return NotFound("Product Not Found");
            }
            ProductContext.Products.Remove(std);
            await ProductContext.SaveChangesAsync();
            return Ok("Product Deleted");

        }
    }
}

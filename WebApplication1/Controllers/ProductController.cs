using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
//using WebApplication1.Migrations;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProducts Iproducts;
        public ProductController(IProducts Iproducts)
        {
            this.Iproducts = Iproducts;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<ActionResult> GetProducts(int page = 1,int pageSize=4)
        {
            var products = await Iproducts.GetAllProducts(page, pageSize);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("GetProduct{id}")]
        public async Task<IActionResult> getProductByid(int id)
        {
            var product = await Iproducts.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult> AddProducts(ProductDTO Productdto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await Iproducts.AddProduct(Productdto);
            if (product == null)
            {
                return BadRequest("Product Category Does Not Exist");
            }
            return Ok(product);
        }



        [HttpPut]
        [Route("UpdateProduct{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDTO Products)
        {
            var product = await Iproducts.UpdateProduct(id,Products);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpDelete]
        [Route("DeleteProduct{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await Iproducts.DeleteProduct(id);
            if (product == null)
            {
                return NotFound("Product is Already Deleted");
            }
            return Ok("Product Deleted Successfully");
        }
    }
}

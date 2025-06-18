using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrders Iorder;
        public OrderController(IOrders Iorder)
        {
            this.Iorder = Iorder;
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await Iorder.GetAllOrder();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO orderDto)
        {
            var order = await Iorder.AddOrder(orderDto);
            if (order == null)
            {
                return NotFound("User or Product Not Found");
            }
            return Ok(order);
        }

        [HttpPut]
        [Route("UpdateOrder{id}")]
        public async Task<ActionResult<Order>> UpdateOrder(int id, OrderDTO order)
        {
            var orders = await Iorder.UpdateOrder(id, order);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("GetOrdersByUser/{id}")]
        public async Task<IActionResult> GetOrdersByUser(int id)
        {
            var orders = await Iorder.GetOrderByUserId(id);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);

        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await Iorder.DeleteOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok("Data Deleted Successfully");
        }
    }
}

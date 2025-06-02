using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Model.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UserContext _context;

        public OrderController(UserContext context) => _context = context;

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            //var exists = await _context.Users.Where(u => !u.Isdeleted);
            //var orders = await _context.Orders
            //    .Include(o => o.User).Where(u=>!u.User.Isdeleted)
            //    .Include(o => o.Product)
            //    .ToListAsync();
            var orders = await _context.Users.Where(u=>!u.Isdeleted).Include(u=>u.Orders)
               //.Include(o => o.Product)
               .ToListAsync();

            return Ok(orders);
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _context.Users.AnyAsync(u => u.ID == orderDto.UserId && !u.Isdeleted);
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == orderDto.ProductId);

            if (!userExists)
                return BadRequest("User does not exist");
            if (!productExists)
                return BadRequest("Product does not exist");

            var order = new Order
            {
                UserId = orderDto.UserId,
                ProductId = orderDto.ProductId,
                //CategoryId=orderDto.CategoryId,
                Quantity = orderDto.Quantity,
                OrderDate = orderDto.OrderDate
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpPut]
        [Route("UpdateOrder{id}")]
        public async Task<ActionResult<Order>> UpdateUser(int id, OrderDTO order)
        {
            var exist = await _context.Orders.FindAsync(id);

            if (exist == null)
            {
                return BadRequest();
            }
            exist.UserId = order.UserId;
            exist.ProductId = order.ProductId;
            exist.Quantity = order.Quantity;
            exist.OrderDate = order.OrderDate;
            await _context.SaveChangesAsync();
            return Ok(order);
        }

        [HttpGet("GetOrdersByUser/{id}")]
        public async Task<IActionResult> GetOrdersByUser(int id)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == id && !o.User.Isdeleted)
                .Include(o => o.Product)
                .ToListAsync();
            if (orders.Count == 0 )
            {
                return NotFound("Order Not Found");
            }
            else
            {
            return Ok(orders);

            }

        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound("Order not found");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Order deleted");
        }
    }
}

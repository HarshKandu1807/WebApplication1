using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Services
{
    public class OrderServices:IOrders
    {
        private readonly UserContext _db;
        public OrderServices(UserContext db)
        {
            this._db = db;
        }

        public async Task<List<Order>?> GetAllOrder()
        {
            var orders = await _db.Orders.Where(u=>!u.User.Isdeleted).Include(o=>o.Product).ToListAsync();
            if (orders == null)
            {
                return null;
            }
            return orders;
        }
        public async Task<List<Order>?> GetOrderByUserId(int id)
        {
            //var order = await _db.Orders.Where(u=>u.UserId==id && !u.User.Isdeleted).Include(o=>o.ProductId).ToListAsync();
            var order = await _db.Orders
                .Where(o => o.UserId == id && !o.User.Isdeleted)
                .Include(o => o.Product)
                .ToListAsync();
            if (order == null)
            {
                return null;
            }
            return order;
        }
        public async Task<OrderDTO?> AddOrder(OrderDTO orderDTO)
        {

            var userId = await _db.Users.AnyAsync(x => x.ID == orderDTO.UserId && !x.Isdeleted);
            if (!userId)
            {
                return null;
            }
            var productId = await _db.Products.AnyAsync(x => x.ProductId == orderDTO.ProductId);
            if (!productId)
            {
                return null;
            }
            var order = new Order
            {
                UserId = orderDTO.UserId,
                ProductId = orderDTO.ProductId,
                Quantity = orderDTO.Quantity,
                OrderDate = DateTime.UtcNow
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return orderDTO;
        }
        public async Task<OrderDTO?> UpdateOrder(int id, OrderDTO orderDTO)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }
            order.UserId = orderDTO.UserId;
            order.ProductId = orderDTO.ProductId;
            order.Quantity = orderDTO.Quantity;
            order.OrderDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return orderDTO;
        }
        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

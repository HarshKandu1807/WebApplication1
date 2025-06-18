using WebApplication1.Model.DTO;

namespace WebApplication1.Services.Iservices
{
    public interface IOrders
    {
        Task<List<Order>?> GetAllOrder();
        Task<List<Order>?> GetOrderByUserId(int id);
        Task<OrderDTO?> AddOrder(OrderDTO orderDTO);
        Task<OrderDTO?> UpdateOrder(int id,OrderDTO orderDTO);
        Task<bool> DeleteOrder(int id);
    }
}

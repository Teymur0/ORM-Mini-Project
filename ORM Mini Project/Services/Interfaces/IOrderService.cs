using ORM_Mini_Project.DTO;
namespace ORM_Mini_Project.Services.Interfaces
{
    internal interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(int userId, decimal totalAmount);
        Task CancelOrderAsync(int orderId);
        Task CompleteOrderAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
    }

}


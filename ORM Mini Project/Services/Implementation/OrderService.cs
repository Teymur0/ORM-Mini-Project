using ORM_Mini_Project.Conexts;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Models.Enums;
using ORM_Mini_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.DTO;
namespace ORM_Mini_Project.Services.Implementation
{
    internal class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, decimal totalAmount)
        {
            if (totalAmount < 0)
            {
                throw new InvalidOrderException("The order amount cannot be less than zero.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOrderException("The user cannot be found.");
            }

            var order = new Order
            {
                UserId = userId,
                TotalAmount = totalAmount,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString()
            };
        }
        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("The order cannot be found.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new OrderAlreadyCancelledException("The order has already been cancelled.");
            }

            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
        }
        public async Task CompleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("The order cannot be found.");
            }

            if (order.Status == OrderStatus.Completed)
            {
                throw new OrderAlreadyCompletedException("The order has already been completed.");
            }

            order.Status = OrderStatus.Completed;
            await _context.SaveChangesAsync();
        }
        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            return orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString()
            }).ToList();
        }
        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new OrderNotFoundException("No order found with the given order ID.");
            }
            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString()
            };
        }

    }
}

using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Conexts;
using ORM_Mini_Project.DTO;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;

namespace ORM_Mini_Project.Services.Implementation
{
    internal class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<PaymentDTO>> GetPaymentsAsync()
        {
            var payments = await _context.Payments.ToListAsync();

            return payments.Select(payment => new PaymentDTO
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate
            }).ToList();
        }
        public async Task MakePaymentAsync(int orderId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidPaymentException("Payment amount must be greater than zero.");
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
    }
}

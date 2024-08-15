using ORM_Mini_Project.DTO;
namespace ORM_Mini_Project.Services.Interfaces
{
    internal interface IPaymentService
    {
        Task MakePaymentAsync(int orderId, decimal amount);
        Task<List<PaymentDTO>> GetPaymentsAsync();
    }
}

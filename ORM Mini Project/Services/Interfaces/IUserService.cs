using ORM_Mini_Project.DTO;

namespace ORM_Mini_Project.Services.Interfaces
{
    internal interface IUserService
    {
        Task RegisterUserAsync(UserDTO user);
        Task<UserDTO> LoginUserAsync(string email, string password);
        Task UpdateUserInfoAsync(UserDTO user);
        Task<List<OrderDTO>> GetUserOrdersAsync(int userId);
        Task ExportUserOrdersToExcelAsync(int userId, string filePath);
    }
}

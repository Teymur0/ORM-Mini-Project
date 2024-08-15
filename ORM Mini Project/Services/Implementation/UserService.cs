using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Conexts;
using ORM_Mini_Project.DTO;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;
namespace ORM_Mini_Project.Services.Implementation
{
    internal class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task RegisterUserAsync(UserDTO userDto)
        {
            var user = new User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                Password = userDto.Password,
                Address = userDto.Address
            };

            if (string.IsNullOrEmpty(user.FullName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new InvalidUserInformationException("Registration information is incomplete.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                throw new UserAuthenticationException("Invalid email or password.");
            }
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address
            };
        }
        public async Task UpdateUserInfoAsync(UserDTO userDto)
        {
            var existingUser = await _context.Users.FindAsync(userDto.Id);

            if (existingUser == null)
            {
                throw new NotFoundException("User not found.");
            }

            existingUser.FullName = userDto.FullName;
            existingUser.Email = userDto.Email;
            existingUser.Password = userDto.Password;
            existingUser.Address = userDto.Address;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }
        public async Task<List<OrderDTO>> GetUserOrdersAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Orders).SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            return user.Orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString()
            }).ToList();
        }

        public async Task ExportUserOrdersToExcelAsync(int userId, string filePath)
        {
            var userOrders = await GetUserOrdersAsync(userId);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");

                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Order Date";
                worksheet.Cell(1, 3).Value = "Total Amount";
                worksheet.Cell(1, 4).Value = "Status";

                for (int i = 0; i < userOrders.Count; i++)
                {
                    var order = userOrders[i];
                    worksheet.Cell(i + 2, 1).Value = order.Id;
                    worksheet.Cell(i + 2, 2).Value = order.OrderDate;
                    worksheet.Cell(i + 2, 3).Value = order.TotalAmount;
                    worksheet.Cell(i + 2, 4).Value = order.Status;
                }

                workbook.SaveAs(filePath);
            }
        }
    }
}

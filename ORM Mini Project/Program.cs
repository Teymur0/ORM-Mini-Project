using ORM_Mini_Project.Conexts;
using ORM_Mini_Project.DTO;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Services.Implementation;
using ORM_Mini_Project.Services.Interfaces;

try
{
    while (true)
    {
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        var choice = Console.ReadLine();

        if (choice == "1")
        {
            await LoginMenuAsync();
        }
        else if (choice == "2")
        {
            await RegisterMenuAsync();
        }
        else if (choice == "3")
        {
            break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
static async Task LoginMenuAsync()
{
    try
    {
        using var context = new AppDbContext();
        IUserService userService = new UserService(context);

        Console.Write("Enter email: ");
        var email = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        var user = await userService.LoginUserAsync(email, password);
        Console.WriteLine("Login successful!");
        await AuthenticatedMenuAsync(userService, user.Id);
    }
    catch (UserAuthenticationException ex)
    {
        Console.WriteLine($"Login failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during login: {ex.Message}");
    }
}
static async Task RegisterMenuAsync()
{
    try
    {
        using var context = new AppDbContext();
        IUserService userService = new UserService(context);

        Console.Write("Enter full name: ");
        var fullName = Console.ReadLine();
        Console.Write("Enter email: ");
        var email = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();
        Console.Write("Enter address: ");
        var address = Console.ReadLine();

        var userDto = new UserDTO
        {
            FullName = fullName,
            Email = email,
            Password = password,
            Address = address
        };

        await userService.RegisterUserAsync(userDto);
        Console.WriteLine("Registration successful!");
    }
    catch (InvalidUserInformationException ex)
    {
        Console.WriteLine($"Registration failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during registration: {ex.Message}");
    }
}
static async Task AuthenticatedMenuAsync(IUserService userService, int userId)
{
    while (true)
    {
        Console.WriteLine("1. View Orders");
        Console.WriteLine("2. Export Orders to Excel");
        Console.WriteLine("3. Manage Products");
        Console.WriteLine("4. Manage Payments");
        Console.WriteLine("5. Logout");
        var choice = Console.ReadLine();

        if (choice == "1")
        {
            await ViewOrdersAsync(userService, userId);
        }
        else if (choice == "2")
        {
            await ExportOrdersToExcelAsync(userService, userId);
        }
        else if (choice == "3")
        {
            await ManageProductsAsync();
        }
        else if (choice == "4")
        {
            await ManagePaymentsAsync();
        }
        else if (choice == "5")
        {
            break;
        }
    }
}
static async Task ViewOrdersAsync(IUserService userService, int userId)
{
    try
    {
        var orders = await userService.GetUserOrdersAsync(userId);
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.Id}, Date: {order.OrderDate}, Total Amount: {order.TotalAmount}, Status: {order.Status}");
        }
    }
    catch (NotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while viewing orders: {ex.Message}");
    }
}
static async Task ExportOrdersToExcelAsync(IUserService userService, int userId)
{
    Console.Write("Enter file path to save Excel file: ");
    var filePath = Console.ReadLine();

    try
    {
        await userService.ExportUserOrdersToExcelAsync(userId, filePath);
        Console.WriteLine("Orders exported to Excel successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error exporting to Excel: {ex.Message}");
    }
}
static async Task ManageProductsAsync()
{
    while (true)
    {
        try
        {
            using var context = new AppDbContext();
            IProductService productService = new ProductService(context);

            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Update Product");
            Console.WriteLine("3. Delete Product");
            Console.WriteLine("4. View Products");
            Console.WriteLine("5. Search Products");
            Console.WriteLine("6. Back");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                await AddProductAsync(productService);
            }
            else if (choice == "2")
            {
                await UpdateProductAsync(productService);
            }
            else if (choice == "3")
            {
                await DeleteProductAsync(productService);
            }
            else if (choice == "4")
            {
                await ViewProductsAsync(productService);
            }
            else if (choice == "5")
            {
                await SearchProductsAsync(productService);
            }
            else if (choice == "6")
            {
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred in Manage Products: {ex.Message}");
        }
    }
}
static async Task ManagePaymentsAsync()
{
    while (true)
    {
        try
        {
            using var context = new AppDbContext();
            IPaymentService paymentService = new PaymentService(context);

            Console.WriteLine("1. Make Payment");
            Console.WriteLine("2. View Payments");
            Console.WriteLine("3. Back");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                await MakePaymentAsync(paymentService);
            }
            else if (choice == "2")
            {
                await ViewPaymentsAsync(paymentService);
            }
            else if (choice == "3")
            {
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred in Manage Payments: {ex.Message}");
        }
    }
}
static async Task AddProductAsync(IProductService productService)
{
    try
    {
        Console.Write("Enter product name: ");
        var name = Console.ReadLine();
        Console.Write("Enter price: ");
        if (!decimal.TryParse(Console.ReadLine(), out var price))
        {
            Console.WriteLine("Invalid price format.");
            return;
        }
        Console.Write("Enter stock: ");
        if (!int.TryParse(Console.ReadLine(), out var stock))
        {
            Console.WriteLine("Invalid stock format.");
            return;
        }
        Console.Write("Enter description: ");
        var description = Console.ReadLine();

        var productDto = new ProductDTO
        {
            Name = name,
            Price = price,
            Stock = stock,
            Description = description
        };

        await productService.AddProductAsync(productDto);
        Console.WriteLine("Product added successfully!");
    }
    catch (InvalidProductException ex)
    {
        Console.WriteLine($"Error adding product: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while adding product: {ex.Message}");
    }
}
static async Task UpdateProductAsync(IProductService productService)
{
    try
    {
        Console.Write("Enter product ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid product ID format.");
            return;
        }

        var existingProduct = await productService.GetProductByIdAsync(id);

        Console.Write("Enter new name: ");
        var name = Console.ReadLine();
        Console.Write("Enter new price: ");
        if (!decimal.TryParse(Console.ReadLine(), out var price))
        {
            Console.WriteLine("Invalid price format.");
            return;
        }
        Console.Write("Enter new stock: ");
        if (!int.TryParse(Console.ReadLine(), out var stock))
        {
            Console.WriteLine("Invalid stock format.");
            return;
        }
        Console.Write("Enter new description: ");
        var description = Console.ReadLine();

        var productDto = new ProductDTO
        {
            Id = id,
            Name = name,
            Price = price,
            Stock = stock,
            Description = description
        };

        await productService.UpdateProductAsync(productDto);
        Console.WriteLine("Product updated successfully!");
    }
    catch (NotFoundException ex)
    {
        Console.WriteLine($"Error updating product: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while updating product: {ex.Message}");
    }
}
static async Task DeleteProductAsync(IProductService productService)
{
    try
    {
        Console.Write("Enter product ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid product ID format.");
            return;
        }

        await productService.DeleteProductAsync(id);
        Console.WriteLine("Product deleted successfully!");
    }
    catch (NotFoundException ex)
    {
        Console.WriteLine($"Error deleting product: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while deleting product: {ex.Message}");
    }
}
static async Task ViewProductsAsync(IProductService productService)
{
    try
    {
        var products = await productService.GetProductsAsync();
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Stock: {product.Stock}, Description: {product.Description}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while viewing products: {ex.Message}");
    }
}
static async Task SearchProductsAsync(IProductService productService)
{
    try
    {
        Console.Write("Enter product name to search: ");
        var name = Console.ReadLine();
        var products = await productService.SearchProductsAsync(name);

        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Stock: {product.Stock}, Description: {product.Description}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while searching products: {ex.Message}");
    }
}
static async Task MakePaymentAsync(IPaymentService paymentService)
{
    try
    {
        Console.Write("Enter order ID: ");
        if (!int.TryParse(Console.ReadLine(), out var orderId))
        {
            Console.WriteLine("Invalid order ID format.");
            return;
        }
        Console.Write("Enter amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out var amount))
        {
            Console.WriteLine("Invalid amount format.");
            return;
        }

        await paymentService.MakePaymentAsync(orderId, amount);
        Console.WriteLine("Payment processed successfully!");
    }
    catch (InvalidPaymentException ex)
    {
        Console.WriteLine($"Error processing payment: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while making payment: {ex.Message}");
    }
}
static async Task ViewPaymentsAsync(IPaymentService paymentService)
{
    try
    {
        var payments = await paymentService.GetPaymentsAsync();
        foreach (var payment in payments)
        {
            Console.WriteLine($"Payment ID: {payment.Id}, Order ID: {payment.OrderId}, Amount: {payment.Amount}, Date: {payment.PaymentDate}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while viewing payments: {ex.Message}");
    }
}
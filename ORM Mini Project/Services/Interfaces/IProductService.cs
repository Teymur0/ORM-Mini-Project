using ORM_Mini_Project.DTO;

namespace ORM_Mini_Project.Services.Interfaces
{
    internal interface IProductService
    {
        Task AddProductAsync(ProductDTO product);
        Task UpdateProductAsync(ProductDTO product);
        Task DeleteProductAsync(int productId);
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task<List<ProductDTO>> GetProductsAsync();
        Task<List<ProductDTO>> SearchProductsAsync(string name);
    }
}

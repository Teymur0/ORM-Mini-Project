using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Conexts;
using ORM_Mini_Project.DTO;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;
namespace ORM_Mini_Project.Services.Implementation
{
    internal class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddProductAsync(ProductDTO productDto)
        {
            if (string.IsNullOrEmpty(productDto.Name) || productDto.Price < 0 || productDto.Stock < 0)
            {
                throw new InvalidProductException("Product name cannot be empty, price must be non-negative, and stock must be non-negative.");
            }

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Description = productDto.Description
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(ProductDTO productDto)
        {
            var existingProduct = await _context.Products.FindAsync(productDto.Id);
            if (existingProduct == null)
                throw new NotFoundException("Product not found.");

            existingProduct.Name = productDto.Name;
            existingProduct.Price = productDto.Price;
            existingProduct.Stock = productDto.Stock;
            existingProduct.Description = productDto.Description;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new NotFoundException("Product not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new NotFoundException("Product not found.");

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            };
        }
        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            }).ToList();
        }
        public async Task<List<ProductDTO>> SearchProductsAsync(string name)
        {
            var products = await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();

            return products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            }).ToList();
        }
    }
}

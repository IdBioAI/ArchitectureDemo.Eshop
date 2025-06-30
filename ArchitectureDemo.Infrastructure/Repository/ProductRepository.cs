using ArchitectureDemo.DbContexts;
using ArchitectureDemo.DbContexts.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureDemo.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(int page, int pageSize);
        Task<List<Product>> GetProducts();
        Task<Product?> GetProductById(int productId);
    }

    public class ProductRepository(ApplicationDbContext applicationDbContext) : IProductRepository
    {
        private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

        public async Task<List<Product>> GetProducts(int page, int pageSize)
        {
            return await applicationDbContext.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<Product>> GetProducts()
        {
            return await applicationDbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await applicationDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
}

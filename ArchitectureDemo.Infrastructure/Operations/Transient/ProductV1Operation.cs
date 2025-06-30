using ArchitectureDemo.DbContexts.Models;
using ArchitectureDemo.Infrastructure.Dto.Product;
using ArchitectureDemo.Infrastructure.Mappers;
using ArchitectureDemo.Infrastructure.Repository;
using Serilog;
using System.Data;


namespace ArchitectureDemo.Infrastructure.Operations.Transient
{
    public interface IProductV1Operation
    {
        Task<List<ProductDto>> GetProducts();
        Task<ProductDto?> GetProductById(int productId);
        Task UpdateProductDescriptionById(int productId, string description);
    }

    public class ProductV1Operation(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductV1Operation
    {
        private readonly IProductRepository productRepository = productRepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<List<ProductDto>> GetProducts()
        {
            var productModels = await productRepository.GetProducts();
            return (productModels ?? Enumerable.Empty<Product>())
                .Select(ProductMapper.MapToProductDto)
                .ToList();
        }

        public async Task<ProductDto?> GetProductById(int productId)
        {

            var productModel = await productRepository.GetProductById(productId);

            if (productModel == null)
            {
                Log.Warning($"Product with ID {productId} not found.");
                return null;
            }

            return ProductMapper.MapToProductDto(productModel);
        }

        public async Task UpdateProductDescriptionById(int productId, string description)
        {
            var productModel = await productRepository.GetProductById(productId);
            if (productModel == null)
            {
                Log.Warning($"Product with ID {productId} not found");
                throw new KeyNotFoundException();
            }

            productModel.Description = description;

            await unitOfWork.CompleteAsync();
        }
    }
}

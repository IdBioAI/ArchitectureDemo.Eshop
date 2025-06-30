using ArchitectureDemo.DbContexts.Models;
using ArchitectureDemo.Infrastructure.Constants;
using ArchitectureDemo.Infrastructure.Dto.Product;
using ArchitectureDemo.Infrastructure.Mappers;
using ArchitectureDemo.Infrastructure.Repository;
using Serilog;

namespace ArchitectureDemo.Infrastructure.Operations.Transient
{
    public interface IProductV2Operation
    {
        Task<List<ProductDto>> GetProducts(ProductRequest productRequest);
    }

    public class ProductV2Operation(IProductRepository productRepository) : IProductV2Operation
    {
        private readonly IProductRepository productRepository = productRepository;

        public async Task<List<ProductDto>> GetProducts(ProductRequest productRequest)
        {
            var productModels = await productRepository.GetProducts(productRequest.Page, productRequest.PageSize ?? SearchConstants.ProductPageSize);
            return (productModels ?? Enumerable.Empty<Product>())
                .Select(ProductMapper.MapToProductDto)
                .ToList();
        }
    }
}

using ArchitectureDemo.DbContexts.Models;
using ArchitectureDemo.Infrastructure.Dto.Product;

namespace ArchitectureDemo.Infrastructure.Mappers
{
    public class ProductMapper
    {
        public static ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ImgUri = product.ImgUri,
                Price = product.Price,
                Description = product.Description
            };
        }
    }
}

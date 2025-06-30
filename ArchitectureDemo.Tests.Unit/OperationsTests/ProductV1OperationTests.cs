using Moq;
using Moq.Language.Flow;
using ArchitectureDemo.Infrastructure.Dto.Product;
using ArchitectureDemo.Infrastructure.Operations.Transient;
using ArchitectureDemo.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using ArchitectureDemo.DbContexts.Models;

namespace ArchitectureDemo.Tests.Unit.OperationsTests
{
    public class ProductV1OperationTests
    {
        private readonly Mock<IProductRepository> productRepository;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly ProductV1Operation productV1Operation;

        public ProductV1OperationTests()
        {
            productRepository = new Mock<IProductRepository>();
            unitOfWork = new Mock<IUnitOfWork>();
            productV1Operation = new ProductV1Operation(productRepository.Object, unitOfWork.Object);
        }

        [Fact]
        public async Task GetProducts()
        {
            var product = new List<Product>
            {
                new() { Id = 16, Name = "Test Product", Description = "Test Description" },
            };

            productRepository.Setup(repo => repo.GetProducts()).ReturnsAsync(product);

            var result = await productV1Operation.GetProducts();
            var returnedDto = Assert.IsType<List<ProductDto>>(result);

            Assert.Equal(product[0].Id, returnedDto[0].Id);
            Assert.Equal(product[0].Name, returnedDto[0].Name);
        }

        [Fact]
        public async Task GetProductById()
        {
            var productId = 16;
            var product = new Product
            {
                Id = productId,
                Name = "Product name 16",
                ImgUri = "https://example.com/image16.jpg",
                Price = 5678.90m,
                Description = "Product description 16"
            };

            productRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(product);

            var result = await productV1Operation.GetProductById(productId);
            var returnedDto = Assert.IsType<ProductDto>(result);

            Assert.Equal(returnedDto.Id, product.Id);
            Assert.Equal(returnedDto.Name, product.Name);
            Assert.Equal(returnedDto.ImgUri, product.ImgUri);
            Assert.Equal(returnedDto.Price, product.Price);
            Assert.Equal(returnedDto.Description, product.Description);
        }

        [Fact]
        public async Task UpdateProductDescriptionById()
        {
            var productId = 1;
            var newDescription = "This is a new description.";
            var product = new Product
            {
                Id = productId,
                Description = "Product description 16"
            };

            productRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(product);

            await productV1Operation.UpdateProductDescriptionById(productId, newDescription);

            Assert.Equal(newDescription, product.Description);
        }
    }
}
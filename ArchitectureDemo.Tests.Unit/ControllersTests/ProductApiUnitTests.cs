using ArchitectureDemo.DbContexts;
using ArchitectureDemo.DbContexts.Models;
using ArchitectureDemo.Infrastructure.Dto.Product;
using ArchitectureDemo.Infrastructure.Operations.Transient;
using ArchitectureDemo.Infrastructure.Repository;
using ArchitectureDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;
using Xunit;
using System.Text;

namespace ArchitectureDemo.Tests.Unit.ControllersTests
{
    public class ProductApiUnitTests(BaseUnitTest baseUnitTest) : IClassFixture<BaseUnitTest>
    {
        private readonly BaseUnitTest baseUnitTest = baseUnitTest;

        [Fact]
        public async Task ApiV1GetProducts()
        {
            // Arrange
            var productV1Operation = new ProductV1Operation(baseUnitTest.ServiceProvider.GetRequiredService<IProductRepository>(),
                                                        baseUnitTest.ServiceProvider.GetRequiredService<IUnitOfWork>());
            var controller = new ProductV1Controller(productV1Operation);

            // Act
            var result = await controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productResponse = Assert.IsType<ProductResponse>(okResult.Value);

            var product = productResponse.ProductList.Skip(10).FirstOrDefault();
            Assert.Equal(11, product?.Id);
            Assert.Equal("Product name 11", product?.Name);
            Assert.Equal("https://example.com/image11.jpg", product?.ImgUri);
            Assert.Equal(789, product?.Price);
            Assert.Equal("Product description 11", product?.Description);
        }

        [Fact]
        public async Task ApiV1GetProductById()
        {
            var productOperation = new ProductV1Operation(baseUnitTest.ServiceProvider.GetRequiredService<IProductRepository>(),
                                                        baseUnitTest.ServiceProvider.GetRequiredService<IUnitOfWork>());
            var controller = new ProductV1Controller(productOperation);


            var result = await controller.GetProductById(16);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(16, product.Id);
            Assert.Equal("Product name 16", product.Name);
            Assert.Equal("https://example.com/image16.jpg", product.ImgUri);
            Assert.Equal(5678.90m, product.Price);
            Assert.Equal("Product description 16", product.Description);
        }

        [Fact]
        public async Task ApiV1UpdateProductDescriptionById()
        {
            int productId = 3;

            var productOperation = new ProductV1Operation(baseUnitTest.ServiceProvider.GetRequiredService<IProductRepository>(),
                                                        baseUnitTest.ServiceProvider.GetRequiredService<IUnitOfWork>());
            var controller = new ProductV1Controller(productOperation);

            // generate short random text
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var res = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                res.Append(chars[random.Next(chars.Length)]);
            }

            
            await controller.UpdateProductDescriptionById(productId, new UpdateDescriptionDto() { Description = res.ToString() });
            var result = await controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<ProductDto>(okResult.Value);

            Assert.Equal(productId, product.Id);
            Assert.Equal(res.ToString(), product.Description);
        }

        [Fact]
        public async Task ApiV2GetProducts()
        {
            // Arrange
            var productV2Operation = new ProductV2Operation(baseUnitTest.ServiceProvider.GetRequiredService<IProductRepository>());
            var controller = new ProductV2Controller(productV2Operation);

            // Act
            var result = await controller.GetProducts(new ProductRequest()
            {
                Page = 2,
                PageSize = 10
            });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productResponse = Assert.IsType<ProductResponse>(okResult.Value);

            var product = productResponse.ProductList.FirstOrDefault();
            Assert.Equal(11, product?.Id);
            Assert.Equal("Product name 11", product?.Name);
            Assert.Equal("https://example.com/image11.jpg", product?.ImgUri);
            Assert.Equal(789, product?.Price);
            Assert.Equal("Product description 11", product?.Description);
        }
    }
}

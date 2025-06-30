using ArchitectureDemo.DbContexts;
using ArchitectureDemo.DbContexts.Models;
using ArchitectureDemo.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ArchitectureDemo.Tests.Unit
{

    public class BaseUnitTest
    {
        public readonly IConfiguration ArchitectureDemoConfiguration;
        public readonly ServiceProvider ServiceProvider;

        private void LoadMockData() 
        {
            // load Product data
            var jsonFilePath = Path.Combine("Products.json");
            var json = File.ReadAllText(jsonFilePath);
            var mockProducts = JsonSerializer.Deserialize<List<Product>>(json);

            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.AddRange(mockProducts);
            context.SaveChanges();
        }

        public BaseUnitTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json");

            ArchitectureDemoConfiguration = builder.Build();

            // dependency injection
            var services = new ServiceCollection();
            services.AddScoped<IProductRepository, ProductRepository>();

            // use mock data from file or real database 
            if (ArchitectureDemoConfiguration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(ArchitectureDemoConfiguration.GetConnectionString("AlzaShopDbContext")));
            }

            ServiceProvider = services.BuildServiceProvider();
            if (ArchitectureDemoConfiguration.GetValue<bool>("UseInMemoryDatabase")) LoadMockData();
        }
    }
}

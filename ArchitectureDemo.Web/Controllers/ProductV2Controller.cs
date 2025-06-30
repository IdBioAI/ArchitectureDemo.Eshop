using ArchitectureDemo.Infrastructure.Dto.Product;
using ArchitectureDemo.Infrastructure.Operations.Transient;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ArchitectureDemo.Web.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("2")]
    public class ProductV2Controller(IProductV2Operation productV2Operation) : ControllerBase
    {

        private readonly IProductV2Operation productV2Operation = productV2Operation;

        /// <summary>
        /// List all available products with pagination support
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts(ProductRequest productRequest)
        {
            var products = await productV2Operation.GetProducts(productRequest);
            var response = new ProductResponse
            {
                ProductList = products
            };

            return Ok(response);
        }
    }
}

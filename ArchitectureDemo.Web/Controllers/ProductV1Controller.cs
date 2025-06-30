using ArchitectureDemo.Infrastructure.Dto.Product;
using ArchitectureDemo.Infrastructure.Operations.Transient;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureDemo.Web.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("1")]
    public class ProductV1Controller(IProductV1Operation productV1Operation) : ControllerBase
    {

        private readonly IProductV1Operation productV1Operation = productV1Operation;

        /// <summary>
        /// List all available products
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(new ProductResponse()
            {
                ProductList = await productV1Operation.GetProducts()
            });
        }

        /// <summary>
        /// Get one product by product Id
        /// </summary>
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await productV1Operation.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Update product description only by product Id
        /// </summary>
        [HttpPatch("{productId}/description")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductDescriptionById(int productId, UpdateDescriptionDto updateDescription)
        {
            try
            {
                await productV1Operation.UpdateProductDescriptionById(productId, updateDescription.Description);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}

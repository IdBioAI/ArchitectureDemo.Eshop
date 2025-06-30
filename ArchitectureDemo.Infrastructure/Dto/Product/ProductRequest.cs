using ArchitectureDemo.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.Infrastructure.Dto.Product
{
    public class ProductRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int Page {  get; set; }

        [Range(1, SearchConstants.MaxProductPageSize, ErrorMessage = "PageSize must be a positive number up to 100.")] 
        public int? PageSize { get; set; }
    }
}

using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{
    public class ProductsSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
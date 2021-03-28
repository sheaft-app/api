using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ProductsSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<SearchProductDto> Products { get; set; }
    }
}
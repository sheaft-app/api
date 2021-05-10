using System.Collections.Generic;
using Sheaft.Domain;

namespace Sheaft.Application.Models
{
    public class ProductsSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
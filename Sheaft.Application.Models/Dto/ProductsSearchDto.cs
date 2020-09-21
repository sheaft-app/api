using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class ProductsSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
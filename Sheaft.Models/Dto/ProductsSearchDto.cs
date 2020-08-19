using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{

    public class ProductsSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
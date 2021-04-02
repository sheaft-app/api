using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CatalogDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDefault { get; set; }
        public int ProductsCount { get; set; }
        public IEnumerable<CatalogProductDto> Products { get; set; }
    }
}
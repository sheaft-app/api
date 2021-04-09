using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class RemoveProductsFromCatalogDto
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class AddProductsToCatalogDto
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
    }
}
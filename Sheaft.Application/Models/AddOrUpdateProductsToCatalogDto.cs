using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class AddOrUpdateProductsToCatalogDto
    {
        public Guid Id { get; set; }
        public IEnumerable<UpdateOrCreateCatalogPriceDto> Products { get; set; }
    }
}
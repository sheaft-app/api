using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class UpdateCatalogPricesDto
    {
        public Guid Id { get; set; }
        public IEnumerable<UpdateOrCreateCatalogPriceDto> Prices { get; set; }
    }
}
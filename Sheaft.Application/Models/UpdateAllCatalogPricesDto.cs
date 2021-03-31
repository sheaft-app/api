using System;
using System.Collections;

namespace Sheaft.Application.Models
{
    public class UpdateAllCatalogPricesDto
    {
        public Guid Id { get; set; }
        public decimal Percent { get; set; }
    }
}
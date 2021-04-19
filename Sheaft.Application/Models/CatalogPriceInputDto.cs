using System;

namespace Sheaft.Application.Models
{
    public class CatalogPriceInputDto
    {
        public Guid CatalogId { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
    }
}
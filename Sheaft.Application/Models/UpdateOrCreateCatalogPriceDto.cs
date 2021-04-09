using System;

namespace Sheaft.Application.Models
{
    public class UpdateOrCreateCatalogPriceDto
    {
        public Guid Id { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
    }
}
using System;

namespace Sheaft.Application.Models
{
    public class ProductPriceInputDto
    {
        public Guid ProductId { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
    }
}
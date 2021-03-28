using System;

namespace Sheaft.Application.Models
{
    public class CatalogPriceDto
    {
        public Guid Id { get; set; }
        public Guid Name { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
        public decimal VatPricePerUnit { get; set; }
        public decimal OnSalePricePerUnit { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal VatPrice { get; set; }
    }
}
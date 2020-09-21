using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
        public decimal VatPricePerUnit { get; set; }
        public decimal OnSalePricePerUnit { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal VatPrice { get; set; }
        public decimal Vat { get; set; }
        public bool Available { get; set; } = true;
        public decimal? Rating { get; set; }
        public Guid? ReturnableId { get; set; }
        public BusinessViewModel Producer { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class ImportedProductDto
    {
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
        public decimal Vat { get; set; }
        public bool Available { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}
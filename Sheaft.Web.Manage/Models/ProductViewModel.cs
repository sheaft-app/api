﻿using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
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
        public string Description { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal Vat { get; set; }
        public bool Available { get; set; } = true;
        public decimal? Rating { get; set; }
        public Guid? ReturnableId { get; set; }
        public ProducerViewModel Producer { get; set; }
        public List<Guid> Tags { get; set; }
        public List<CatalogPriceViewModel> CatalogsPrices { get; set; }
        public List<PictureViewModel> Pictures { get; set; }
    }
}

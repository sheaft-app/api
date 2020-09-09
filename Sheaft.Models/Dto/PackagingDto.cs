﻿using System;

namespace Sheaft.Models.Dto
{
    public class PackagingDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
        public decimal VatPrice { get; set; }
        public decimal OnSalePrice { get; set; }
    }
}
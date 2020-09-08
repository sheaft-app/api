﻿using System;

namespace Sheaft.Models.Dto
{
    public class QuickOrderProductQuantityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public PackagingDto Packaging { get; set; }
        public int? Quantity { get; set; }
        public decimal Vat { get; set; }
        public decimal UnitWholeSalePrice { get; set; }
        public decimal UnitVatPrice { get; set; }
        public decimal UnitOnSalePrice { get; set; }
        public decimal? UnitWeight { get; set; }
        public UserDto Producer { get; set; }
    }
}

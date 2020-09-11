using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class PurchaseOrderProductQuantityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public int Quantity { get; set; }
        public decimal Vat { get; set; }
        public decimal UnitWholeSalePrice { get; set; }
        public decimal UnitVatPrice { get; set; }
        public decimal UnitOnSalePrice { get; set; }
        public decimal? UnitWeight { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal? TotalWeight { get; set; }
        public string ReturnableName { get; set; }
        public decimal? ReturnableVat { get; set; }
        public decimal? ReturnableVatPrice { get; set; }
        public decimal? ReturnableWholeSalePrice { get; set; }
        public decimal? ReturnableOnSalePrice { get; set; }
    }
}

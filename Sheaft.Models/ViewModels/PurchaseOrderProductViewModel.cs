using System;

namespace Sheaft.Models.ViewModels
{
    public class PurchaseOrderProductViewModel
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
        public string PackagingName { get; set; }
        public decimal? PackagingVat { get; set; }
        public decimal? PackagingVatPrice { get; set; }
        public decimal? PackagingWholeSalePrice { get; set; }
        public decimal? PackagingOnSalePrice { get; set; }
    }
}

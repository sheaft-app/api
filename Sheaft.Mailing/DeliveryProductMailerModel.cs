using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Mailing
{
    public class DeliveryProductMailerModel
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public decimal ProductWholeSalePrice { get; set; }
        public decimal ProductTotalWholeSalePrice { get; set; }
        public decimal ProductTotalOnSalePrice { get; set; }
        public decimal ProductTotalVatPrice { get; set; }
        public decimal Vat { get; set; }
        public int Quantity { get; set; }
        public ModificationKind RowKind { get; set; }
        
        public bool HasReturnable { get; set; }
        public string ReturnableName { get; set; }
        public int? ReturnableQuantity { get; set; }
        public decimal? ReturnableVat { get; set; }
        public decimal? ReturnableTotalVatPrice { get; set; }
        public decimal? ReturnableTotalOnSalePrice { get; set; }
        public decimal? ReturnableWholeSalePrice { get; set; }
        public decimal? ReturnableTotalWholeSalePrice { get; set; }
        
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
    }
}
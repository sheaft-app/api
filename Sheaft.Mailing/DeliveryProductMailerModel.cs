using Sheaft.Domain.Enum;

namespace Sheaft.Mailing
{
    public class DeliveryProductMailerModel
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public decimal UnitWholeSalePrice { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal Vat { get; set; }
        public int Quantity { get; set; }
        public ModificationKind RowKind { get; set; }
    }
}
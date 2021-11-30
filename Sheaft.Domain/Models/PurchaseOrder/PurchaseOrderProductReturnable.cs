using System;

namespace Sheaft.Domain
{
    public class PurchaseOrderProductReturnable
    {
        protected PurchaseOrderProductReturnable()
        {
        }

        public PurchaseOrderProductReturnable(Returnable returnable)
        {
            Name = returnable.Name;
            WholeSalePrice = returnable.WholeSalePrice;
            Vat = returnable.Vat;
            ReturnableId = returnable.Id;
        }

        public string Name { get; private set; }
        public decimal Vat { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public Guid ReturnableId { get; private set; }
    }
}
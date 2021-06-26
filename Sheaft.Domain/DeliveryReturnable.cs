using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryReturnable : IIdEntity, ITrackCreation, ITrackUpdate
    {
        private const int DIGITS_COUNT = 2;
        
        protected DeliveryReturnable()
        {
        }

        public DeliveryReturnable(Returnable returnable, int quantity)
        {
            Id = Guid.NewGuid();
            Kind = returnable.Kind;
            Name = returnable.Name;
            Vat = returnable.Vat;
            UnitVatPrice = returnable.VatPrice;
            UnitWholeSalePrice = returnable.WholeSalePrice;
            UnitOnSalePrice = returnable.OnSalePrice;
            ReturnableId = returnable.Id;
            Quantity = -quantity;
            
            RefreshLine();
        }
        
        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set;}
        public DateTimeOffset? UpdatedOn { get; private set;}
        public int Quantity { get; private set; }
        public ReturnableKind Kind { get; private set; }
        public string Name { get; private set; }
        public decimal Vat { get; private set; }
        public decimal UnitWholeSalePrice { get; private set; }
        public decimal UnitVatPrice { get; private set; }
        public decimal UnitOnSalePrice { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public Guid ReturnableId { get; private set;}
        public Guid DeliveryId { get; private set;}
        public byte[] RowVersion { get; set; }

        public void SetQuantity(int quantity)
        {
            if(quantity < 0)
                throw SheaftException.Validation();
            
            Quantity = quantity;
            RefreshLine();
        }

        private void RefreshLine()
        {
            TotalVatPrice = Math.Round(UnitVatPrice * Quantity, DIGITS_COUNT);
            TotalWholeSalePrice = Math.Round(UnitWholeSalePrice * Quantity, DIGITS_COUNT);
            TotalOnSalePrice = Math.Round(UnitWholeSalePrice * Quantity, DIGITS_COUNT);
        }
    }
}
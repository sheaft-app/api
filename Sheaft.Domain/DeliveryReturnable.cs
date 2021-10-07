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
            UnitWholeSalePrice = returnable.WholeSalePrice;
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
        public decimal UnitVatPrice => Math.Round(UnitWholeSalePrice * Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal UnitOnSalePrice => Math.Round(UnitWholeSalePrice * (1+Vat / 100), 2, MidpointRounding.AwayFromZero);
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public Guid ReturnableId { get; private set;}
        public Guid DeliveryId { get; private set;}
        public byte[] RowVersion { get; set; }

        public void SetQuantity(int quantity)
        {
            if(quantity < 0)
                throw SheaftException.Validation("La quantité de consigne retournée ne peut pas être négative.");
            
            Quantity = quantity;
            RefreshLine();
        }

        private void RefreshLine()
        {
            TotalVatPrice = Math.Round(UnitWholeSalePrice * Quantity * Vat / 100, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            TotalWholeSalePrice = Math.Round(UnitWholeSalePrice * Quantity, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            TotalOnSalePrice = Math.Round(UnitWholeSalePrice * Quantity * (1 + Vat / 100), DIGITS_COUNT, MidpointRounding.AwayFromZero);
        }
    }
}
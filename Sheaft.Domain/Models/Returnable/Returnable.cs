using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Returnable : IEntity
    {
        private const int DIGITS_COUNT = 2;

        protected Returnable()
        {
        }

        public Returnable(Company supplier, string name, decimal wholeSalePrice, decimal vat, string description = null)
        {
            Name = name;
            Description = description;
            Supplier = supplier;
            SupplierId = supplier.Id;
            
            SetWholeSalePrice(wholeSalePrice);
            SetVat(vat);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public decimal Vat { get; private set; }
        public decimal VatPrice => Math.Round(WholeSalePrice * Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal OnSalePrice => Math.Round(WholeSalePrice * (1+Vat / 100), 2, MidpointRounding.AwayFromZero);
        public Guid SupplierId { get; private set; }
        public Company Supplier { get; private set; }
        
        public void Restore()
        {
            Removed = false;
        }

        public void SetWholeSalePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ValidationException("Le prix doit être supérieur à 0€.");

            WholeSalePrice = Math.Round(newPrice, DIGITS_COUNT, MidpointRounding.AwayFromZero);
        }

        public void SetVat(decimal newVat)
        {
            if (newVat < 0)
                throw new ValidationException("La TVA doit être supérieure à 0%.");

            if (newVat > 100)
                throw new ValidationException("La TVA doit être inférieure à 100%.");

            Vat = newVat;
        }
    }
}
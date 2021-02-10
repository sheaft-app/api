using System;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public class Returnable : IEntity
    {
        private const int DIGITS_COUNT = 2;

        protected Returnable()
        {
        }

        public Returnable(Guid id, ReturnableKind kind, Producer producer, string name, decimal wholeSalePrice, decimal vat, string description = null)
        {
            Id = id;
            Description = description;
            Producer = producer;
            Kind = kind;

            SetName(name);
            SetWholeSalePrice(wholeSalePrice);
            SetVat(vat);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public ReturnableKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public decimal Vat { get; private set; }
        public decimal VatPrice { get; private set; }
        public decimal OnSalePrice { get; private set; }
        public virtual Producer Producer { get; private set; }

        public void SetWholeSalePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ValidationException(MessageKind.Returnable_WholeSalePrice_CannotBe_LowerThan, 0);

            WholeSalePrice = Math.Round(newPrice, DIGITS_COUNT);
            RefreshPrices();
        }

        public void SetVat(decimal newVat)
        {
            if (newVat < 0)
                throw new ValidationException(MessageKind.Returnable_Vat_CannotBe_LowerThan, 0);

            if (newVat > 100)
                throw new ValidationException(MessageKind.Returnable_Vat_CannotBe_GreaterThan, 100);

            Vat = newVat;
            RefreshPrices();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Returnable_Name_Required);

            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        protected void RefreshPrices()
        {
            VatPrice = Math.Round(WholeSalePrice * Vat / 100, DIGITS_COUNT);
            OnSalePrice = Math.Round(WholeSalePrice + VatPrice, DIGITS_COUNT);
        }
    }
}
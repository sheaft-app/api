using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
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
            ProducerId = producer.Id;
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
        public decimal VatPrice => Math.Round(WholeSalePrice * Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal OnSalePrice => Math.Round(WholeSalePrice * (1+Vat / 100), 2, MidpointRounding.AwayFromZero);
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetWholeSalePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw SheaftException.Validation("Le prix doit être supérieur à 0€.");

            WholeSalePrice = Math.Round(newPrice, DIGITS_COUNT, MidpointRounding.AwayFromZero);
        }

        public void SetVat(decimal newVat)
        {
            if (newVat < 0)
                throw SheaftException.Validation("La TVA doit être supérieure à 0%.");

            if (newVat > 100)
                throw SheaftException.Validation("La TVA doit être inférieure à 100%.");

            Vat = newVat;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");

            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}
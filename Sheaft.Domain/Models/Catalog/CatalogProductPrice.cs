using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class CatalogProductPrice : ITrackCreation, ITrackUpdate
    {
        private const int DIGITS_COUNT = 2;

        protected CatalogProductPrice()
        {
        }

        public CatalogProductPrice(Product product, Catalog catalog, decimal wholeSalePrice)
        {
            Id = Guid.NewGuid();
            ProductId = product.Id;
            CatalogId = catalog.Id;
            Product = product;
            
            SetWholeSalePrice(wholeSalePrice);
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public decimal VatPrice => Math.Round(WholeSalePrice * Product.Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal OnSalePrice => Math.Round(WholeSalePrice * (1+Product.Vat / 100), 2, MidpointRounding.AwayFromZero);
        public decimal WholeSalePricePerUnit { get; private set; }
        public decimal VatPricePerUnit => Math.Round(WholeSalePricePerUnit * Product.Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal OnSalePricePerUnit => Math.Round(WholeSalePricePerUnit * (1+Product.Vat / 100), 2, MidpointRounding.AwayFromZero);
        public Guid CatalogId { get; }
        public Guid ProductId { get; }
        public Product Product { get; }
        public byte[] RowVersion { get; private set; }

        public void SetWholeSalePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ValidationException("Le prix d'un produit du catalogue ne peut être inférieur à 0€.");

            WholeSalePrice = Math.Round(newPrice, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            RefreshPrice(Product.Unit, Product.QuantityPerUnit);
        }

        public void RefreshPrice(UnitKind unit, decimal quantityPerUnit)
        {
            switch (unit)
            {
                case UnitKind.ml:
                    WholeSalePricePerUnit = Math.Round(WholeSalePrice / quantityPerUnit * 1000, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                case UnitKind.l:
                    WholeSalePricePerUnit = Math.Round(WholeSalePrice / quantityPerUnit, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                case UnitKind.g:
                    WholeSalePricePerUnit = Math.Round(WholeSalePrice / quantityPerUnit * 1000, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                case UnitKind.kg:
                    WholeSalePricePerUnit = Math.Round(WholeSalePrice / quantityPerUnit, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                default:
                    WholeSalePricePerUnit = WholeSalePrice;
                    break;
            }
        }
    }
}
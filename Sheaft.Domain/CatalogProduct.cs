using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class CatalogProduct : ITrackCreation, ITrackUpdate
    {
        private const int DIGITS_COUNT = 2;

        protected CatalogProduct()
        {
        }

        public CatalogProduct(Guid id, Product product, Catalog catalog, decimal wholeSalePrice)
        {
            Id = id;
            Product = product;
            ProductId = product.Id;
            Catalog = catalog;
            CatalogId = catalog.Id;
            SetWholeSalePricePerUnit(wholeSalePrice);
            
            catalog.IncreaseProductsCount();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public decimal VatPrice => Math.Round(WholeSalePrice * Product.Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal OnSalePrice => Math.Round(WholeSalePrice * (1+Product.Vat / 100), 2, MidpointRounding.AwayFromZero);
        public decimal WholeSalePricePerUnit { get; private set; }
        public decimal VatPricePerUnit => Math.Round(WholeSalePricePerUnit * Product.Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal OnSalePricePerUnit => Math.Round(WholeSalePricePerUnit * (1+Product.Vat / 100), 2, MidpointRounding.AwayFromZero);
        public Guid CatalogId { get; private set; }
        public Guid ProductId { get; private set; }
        public virtual Product Product { get; private set; }
        public virtual Catalog Catalog { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetWholeSalePricePerUnit(decimal newPrice)
        {
            if (newPrice < 0)
                throw SheaftException.Validation("Le prix d'un produit du catalogue ne peut être inférieur à 0€.");

            WholeSalePricePerUnit = Math.Round(newPrice, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            RefreshPrice(Product.Vat, Product.Unit, Product.QuantityPerUnit);
        }

        public void RefreshPrice(decimal vat, UnitKind unit, decimal quantityPerUnit)
        {
            switch (unit)
            {
                case UnitKind.ml:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit * 1000, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                case UnitKind.l:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                case UnitKind.g:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit * 1000, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                case UnitKind.kg:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit, DIGITS_COUNT, MidpointRounding.AwayFromZero);
                    break;
                default:
                    WholeSalePrice = WholeSalePricePerUnit;
                    break;
            }
        }

        public void UpdatePrice(decimal percent)
        {
            SetWholeSalePricePerUnit(WholeSalePricePerUnit * (1 + percent / 100));
        }
    }
}
using System;
using Sheaft.Core.Enums;
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

        public CatalogProduct(Product product, Catalog catalog, decimal wholeSalePrice)
        {
            Product = product;
            Catalog = catalog;
            SetWholeSalePricePerUnit(wholeSalePrice);
        }

        public DateTimeOffset CreatedOn { get; }
        public DateTimeOffset? UpdatedOn { get; }
        public decimal OnSalePrice { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public decimal VatPrice { get; private set; }
        public decimal WholeSalePricePerUnit { get; private set; }
        public decimal VatPricePerUnit { get; private set; }
        public decimal OnSalePricePerUnit { get; private set; }
        public virtual Product Product { get; }
        public virtual Catalog Catalog { get; }

        public void SetWholeSalePricePerUnit(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ValidationException(MessageKind.Product_WholeSalePrice_CannotBe_LowerOrEqualThan, 0);

            WholeSalePricePerUnit = Math.Round(newPrice, DIGITS_COUNT);
            RefreshPrice(Product.Vat, Product.Unit, Product.QuantityPerUnit);
        }

        public void RefreshPrice(decimal vat, UnitKind unit, decimal quantityPerUnit)
        {
            VatPricePerUnit = Math.Round(WholeSalePricePerUnit * vat / 100, DIGITS_COUNT);
            OnSalePricePerUnit = Math.Round(WholeSalePricePerUnit + VatPricePerUnit, DIGITS_COUNT);

            switch (unit)
            {
                case UnitKind.ml:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit * 1000, DIGITS_COUNT);
                    break;
                case UnitKind.l:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit, DIGITS_COUNT);
                    break;
                case UnitKind.g:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit * 1000, DIGITS_COUNT);
                    break;
                case UnitKind.kg:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / quantityPerUnit, DIGITS_COUNT);
                    break;
                default:
                    WholeSalePrice = WholeSalePricePerUnit;
                    break;
            }

            VatPrice = Math.Round(WholeSalePrice * vat / 100, DIGITS_COUNT);
            OnSalePrice = Math.Round(WholeSalePrice + VatPrice, DIGITS_COUNT);
        }
    }
}
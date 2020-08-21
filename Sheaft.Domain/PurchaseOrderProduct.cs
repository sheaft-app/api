using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class PurchaseOrderProduct
    {
        private const int DIGITS_COUNT = 2;

        protected PurchaseOrderProduct()
        {
        }

        public PurchaseOrderProduct(Product product, int quantity)
        {
            UnitWholeSalePrice = product.WholeSalePricePerUnit;
            UnitOnSalePrice = product.OnSalePricePerUnit;
            UnitVatPrice = product.VatPricePerUnit;
            UnitWeight = product.Weight;

            Id = product.Id;
            Name = product.Name;
            Reference = product.Reference;

            PackagingName = product.Packaging?.Name;
            PackagingVat = product.Packaging?.Vat;
            PackagingVatPrice = product.Packaging?.VatPrice;
            PackagingWholeSalePrice = product.Packaging?.WholeSalePrice;
            PackagingOnSalePrice = product.Packaging?.OnSalePrice;

            SetQuantity(quantity);
        }

        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ValidationException(MessageKind.PurchaseOrder_ProductQuantity_CannotBe_LowerOrEqualThan, 0);

            Quantity = quantity;
            RefreshLine();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Reference { get; private set; }
        public int Quantity { get; private set; }
        public decimal Vat { get; private set; }
        public decimal UnitWholeSalePrice { get; private set; }
        public decimal UnitVatPrice { get; private set; }
        public decimal UnitOnSalePrice { get; private set; }
        public decimal? UnitWeight { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal? TotalWeight { get; private set; }
        public string PackagingName { get; private set; }
        public decimal? PackagingOnSalePrice { get; private set; }
        public decimal? PackagingWholeSalePrice { get; private set; }
        public decimal? PackagingVatPrice { get; private set; }
        public decimal? PackagingVat { get; private set; }

        protected void RefreshLine()
        {
            TotalVatPrice = Math.Round(Quantity * (UnitVatPrice + PackagingVatPrice ?? 0), DIGITS_COUNT);
            TotalWholeSalePrice = Math.Round(Quantity * (UnitWholeSalePrice + PackagingWholeSalePrice ?? 0), DIGITS_COUNT);
            TotalOnSalePrice = Math.Round(TotalWholeSalePrice + TotalVatPrice, DIGITS_COUNT);
            TotalWeight = UnitWeight.HasValue ? Math.Round(Quantity * UnitWeight.Value, DIGITS_COUNT) : (decimal?)null;
        }
    }
}
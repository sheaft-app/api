﻿using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class ProductRow
    {
        private const int DIGITS_COUNT = 2;

        protected ProductRow()
        {
        }

        protected ProductRow(ProductRow product)
        {
            UnitWholeSalePrice = product.UnitWholeSalePrice;
            UnitOnSalePrice = product.UnitOnSalePrice;
            UnitVatPrice = product.UnitVatPrice;
            UnitWeight = product.UnitWeight;
            Vat = product.Vat;

            Id = product.Id;
            Name = product.Name;
            Reference = product.Reference;

            ReturnableName = product.ReturnableName;
            ReturnableVat = product.ReturnableVat;
            ReturnableVatPrice = product.ReturnableVatPrice;
            ReturnableWholeSalePrice = product.ReturnableWholeSalePrice;
            ReturnableOnSalePrice = product.ReturnableOnSalePrice;
            ReturnablesCount = product.ReturnablesCount;

            Quantity = product.Quantity;
            TotalWeight = product.TotalWeight;

            TotalProductVatPrice = product.TotalProductVatPrice;
            TotalProductWholeSalePrice = product.TotalProductWholeSalePrice;
            TotalProductOnSalePrice = product.TotalProductOnSalePrice;

            TotalReturnableVatPrice = product.TotalReturnableVatPrice;
            TotalReturnableWholeSalePrice = product.TotalReturnableWholeSalePrice;
            TotalReturnableOnSalePrice = product.TotalReturnableOnSalePrice;

            TotalVatPrice = product.TotalVatPrice;
            TotalWholeSalePrice = product.TotalWholeSalePrice;
            TotalOnSalePrice = product.TotalOnSalePrice;
        }

        protected ProductRow(Product product, int quantity)
        {
            UnitWholeSalePrice = product.WholeSalePricePerUnit;
            UnitOnSalePrice = product.OnSalePricePerUnit;
            UnitVatPrice = product.VatPricePerUnit;
            UnitWeight = product.Weight;
            Vat = product.Vat;

            Id = product.Id;
            Name = product.Name;
            Reference = product.Reference;

            ReturnableName = product.Returnable?.Name;
            ReturnableVat = product.Returnable?.Vat;
            ReturnableVatPrice = product.Returnable?.VatPrice;
            ReturnableWholeSalePrice = product.Returnable?.WholeSalePrice;
            ReturnableOnSalePrice = product.Returnable?.OnSalePrice;

            SetQuantity(quantity);
        }

        private void SetQuantity(int quantity)
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
        public decimal TotalProductWholeSalePrice { get; private set; }
        public decimal TotalProductVatPrice { get; private set; }
        public decimal TotalProductOnSalePrice { get; private set; }
        public decimal? TotalWeight { get; private set; }
        public int ReturnablesCount { get; private set; }
        public string ReturnableName { get; private set; }
        public decimal? ReturnableOnSalePrice { get; private set; }
        public decimal? ReturnableWholeSalePrice { get; private set; }
        public decimal? ReturnableVatPrice { get; private set; }
        public decimal? ReturnableVat { get; private set; }
        public decimal? TotalReturnableWholeSalePrice { get; private set; }
        public decimal? TotalReturnableVatPrice { get; private set; }
        public decimal? TotalReturnableOnSalePrice { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }

        protected void RefreshLine()
        {
            TotalProductVatPrice = Math.Round(Quantity * UnitVatPrice, DIGITS_COUNT);
            TotalProductWholeSalePrice = Math.Round(Quantity * UnitWholeSalePrice, DIGITS_COUNT);
            TotalProductOnSalePrice = Math.Round(TotalProductVatPrice + TotalProductWholeSalePrice, DIGITS_COUNT);

            ReturnablesCount = ReturnableVat.HasValue ? Quantity : 0;

            TotalReturnableVatPrice = ReturnablesCount > 0 ? Math.Round(ReturnablesCount * ReturnableVatPrice.Value, DIGITS_COUNT) : (decimal?)null;
            TotalReturnableWholeSalePrice = ReturnablesCount > 0 ? Math.Round(ReturnablesCount * ReturnableWholeSalePrice.Value, DIGITS_COUNT) : (decimal?)null;
            TotalReturnableOnSalePrice = ReturnablesCount > 0 ? Math.Round(TotalReturnableVatPrice.Value  + TotalReturnableWholeSalePrice.Value, DIGITS_COUNT) : (decimal?)null;

            TotalWeight = UnitWeight.HasValue ? Math.Round(Quantity * UnitWeight.Value, DIGITS_COUNT) : (decimal?)null;

            TotalVatPrice = Math.Round(TotalProductVatPrice + (TotalReturnableVatPrice ?? 0), DIGITS_COUNT);
            TotalWholeSalePrice = Math.Round(TotalProductWholeSalePrice + (TotalReturnableWholeSalePrice ?? 0), DIGITS_COUNT);
            TotalOnSalePrice = Math.Round(TotalVatPrice + TotalWholeSalePrice, DIGITS_COUNT);
        }
    }
}
using System;
using System.Linq;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class ProductRow: IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected const int DIGITS_COUNT = 2;

        protected ProductRow()
        {
        }

        protected ProductRow(ProductRow product)
        {
            Unit = product.Unit;
            Conditioning = product.Conditioning;
            QuantityPerUnit = product.QuantityPerUnit;
            UnitWholeSalePrice = product.UnitWholeSalePrice;
            UnitWeight = product.UnitWeight;
            Vat = product.Vat;

            Id = Guid.NewGuid();
            Name = product.Name;
            Reference = product.Reference;

            ReturnableId = product.ReturnableId;
            ReturnableName = product.ReturnableName;
            ReturnableVat = product.ReturnableVat;
            ReturnableWholeSalePrice = product.ReturnableWholeSalePrice;
            HasReturnable = product.ReturnableWholeSalePrice.HasValue;

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

            Quantity = product.Quantity;
            ProductId = product.ProductId;
            HasReturnable = ReturnableId.HasValue;
        }
        
        protected ProductRow(ProductRow product, int quantity)
        {
            Unit = product.Unit;
            Conditioning = product.Conditioning;
            QuantityPerUnit = product.QuantityPerUnit;
            UnitWholeSalePrice = product.UnitWholeSalePrice;
            UnitWeight = product.UnitWeight;
            Vat = product.Vat;

            Id = Guid.NewGuid();
            Name = product.Name;
            Reference = product.Reference;

            ReturnableId = product.ReturnableId;
            ReturnableName = product.ReturnableName;
            ReturnableVat = product.ReturnableVat;
            ReturnableWholeSalePrice = product.ReturnableWholeSalePrice;
            HasReturnable = product.ReturnableWholeSalePrice.HasValue;

            ProductId = product.ProductId;
            HasReturnable = ReturnableId.HasValue;
            
            SetQuantity(quantity);
        }

        protected ProductRow(Product product, Guid catalogId, int quantity)
        {
            var productPrice = product.CatalogsPrices.Single(p => p.CatalogId == catalogId);
            
            Unit = product.Unit;
            Conditioning = product.Conditioning;
            QuantityPerUnit = product.QuantityPerUnit;
            UnitWholeSalePrice = productPrice.WholeSalePricePerUnit;
            UnitWeight = product.Weight;
            Vat = product.Vat;

            Id = Guid.NewGuid();
            Name = product.Name;
            Reference = product.Reference;

            ReturnableId = product.ReturnableId;
            ReturnableName = product.Returnable?.Name;
            ReturnableVat = product.Returnable?.Vat;
            ReturnableWholeSalePrice = product.Returnable?.WholeSalePrice;

            ProductId = product.Id;
            HasReturnable = ReturnableId.HasValue;
            
            SetQuantity(quantity);
        }

        public virtual void SetQuantity(int quantity)
        {
            Quantity = quantity;
            RefreshLine();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Name { get; private set; }
        public string Reference { get; private set; }
        public int Quantity { get; protected set; }
        public decimal Vat { get; private set; }
        public decimal UnitWholeSalePrice { get; private set; }
        public decimal UnitVatPrice => Math.Round(UnitWholeSalePrice * Vat / 100, 2, MidpointRounding.AwayFromZero);
        public decimal UnitOnSalePrice => Math.Round(UnitWholeSalePrice * (1+Vat / 100), 2, MidpointRounding.AwayFromZero);
        public decimal? UnitWeight { get; private set; }
        public decimal TotalProductWholeSalePrice { get; private set; }
        public decimal TotalProductVatPrice { get; private set; }
        public decimal TotalProductOnSalePrice { get; private set; }
        public decimal? TotalWeight { get; private set; }
        public bool HasReturnable { get; private set; }
        public Guid? ReturnableId { get; private set; }
        public string ReturnableName { get; private set; }
        public decimal? ReturnableWholeSalePrice { get; private set; }
        public decimal? ReturnableVatPrice => Math.Round(ReturnableWholeSalePrice ?? 0m * ReturnableVat ?? 0m / 100, 2, MidpointRounding.AwayFromZero);
        public decimal? ReturnableOnSalePrice => Math.Round(ReturnableWholeSalePrice ?? 0m * (1+ReturnableVat ?? 0m / 100), 2, MidpointRounding.AwayFromZero);
        public decimal? ReturnableVat { get; private set; }
        public decimal? TotalReturnableWholeSalePrice { get; private set; }
        public decimal? TotalReturnableVatPrice { get; private set; }
        public decimal? TotalReturnableOnSalePrice { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public Guid ProductId { get; private set; }
        public byte[] RowVersion { get; private set; }

        protected void RefreshLine()
        {
            TotalProductWholeSalePrice = Math.Round(Quantity * UnitWholeSalePrice, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            TotalProductVatPrice = Math.Round(Quantity * UnitWholeSalePrice * Vat / 100, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            TotalProductOnSalePrice = Math.Round(Quantity * UnitWholeSalePrice * (1 + Vat / 100), DIGITS_COUNT, MidpointRounding.AwayFromZero);

            TotalReturnableWholeSalePrice = HasReturnable ? Math.Round(Quantity * ReturnableWholeSalePrice.Value, DIGITS_COUNT, MidpointRounding.AwayFromZero) : (decimal?)null;
            TotalReturnableVatPrice = HasReturnable ? Math.Round(Quantity * ReturnableWholeSalePrice.Value * ReturnableVat.Value / 100, DIGITS_COUNT, MidpointRounding.AwayFromZero) : (decimal?)null;
            TotalReturnableOnSalePrice = HasReturnable ? Math.Round(Quantity * ReturnableWholeSalePrice.Value * (1+ ReturnableVat.Value /100), DIGITS_COUNT, MidpointRounding.AwayFromZero) : (decimal?)null;

            TotalWeight = UnitWeight.HasValue ? Math.Round(Quantity * UnitWeight.Value, DIGITS_COUNT, MidpointRounding.AwayFromZero) : (decimal?)null;

            TotalVatPrice = TotalProductVatPrice + (TotalReturnableVatPrice ?? 0);
            TotalWholeSalePrice = TotalProductWholeSalePrice + (TotalReturnableWholeSalePrice ?? 0);
            TotalOnSalePrice = TotalVatPrice + TotalWholeSalePrice;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
            RefreshLine();
        }

        public void RemoveQuantity(int quantity)
        {
            Quantity -= quantity;
            if (Quantity < 0)
                Quantity = 0;
            
            RefreshLine();
        }
    }

    public static class ProductRowExtensions
    {
        
        public static string GetConditioning(this ProductRow productRow)
        {
            switch (productRow.Conditioning)
            {
                case ConditioningKind.Basket:
                    return $"Panier pour {productRow.QuantityPerUnit:0} personnes";
                case ConditioningKind.Bouquet:
                    return $"{productRow.QuantityPerUnit:0} bouquet(s)";
                case ConditioningKind.Box:
                    return $"Boite de {productRow.QuantityPerUnit:0}";
                case ConditioningKind.Bulk:
                    return $"{productRow.QuantityPerUnit:0}{productRow.Unit:G}";
                case ConditioningKind.Piece:
                    return $"{productRow.QuantityPerUnit:0} pièce(s)";
                case ConditioningKind.Bunch:
                    return $"{productRow.QuantityPerUnit:0} botte(s)";
                default:
                    return string.Empty;
            }
        }
    }
}
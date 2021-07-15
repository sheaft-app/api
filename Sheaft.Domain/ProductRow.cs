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
            UnitOnSalePrice = product.UnitOnSalePrice;
            UnitVatPrice = product.UnitVatPrice;
            UnitWeight = product.UnitWeight;
            Vat = product.Vat;

            Id = Guid.NewGuid();
            Name = product.Name;
            Reference = product.Reference;

            ReturnableId = product.ReturnableId;
            ReturnableName = product.ReturnableName;
            ReturnableVat = product.ReturnableVat;
            ReturnableVatPrice = product.ReturnableVatPrice;
            ReturnableWholeSalePrice = product.ReturnableWholeSalePrice;
            ReturnableOnSalePrice = product.ReturnableOnSalePrice;
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
            UnitOnSalePrice = product.UnitOnSalePrice;
            UnitVatPrice = product.UnitVatPrice;
            UnitWeight = product.UnitWeight;
            Vat = product.Vat;

            Id = Guid.NewGuid();
            Name = product.Name;
            Reference = product.Reference;

            ReturnableId = product.ReturnableId;
            ReturnableName = product.ReturnableName;
            ReturnableVat = product.ReturnableVat;
            ReturnableVatPrice = product.ReturnableVatPrice;
            ReturnableWholeSalePrice = product.ReturnableWholeSalePrice;
            ReturnableOnSalePrice = product.ReturnableOnSalePrice;
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
            UnitOnSalePrice = productPrice.OnSalePricePerUnit;
            UnitVatPrice = productPrice.VatPricePerUnit;
            UnitWeight = product.Weight;
            Vat = product.Vat;

            Id = Guid.NewGuid();
            Name = product.Name;
            Reference = product.Reference;

            ReturnableId = product.ReturnableId;
            ReturnableName = product.Returnable?.Name;
            ReturnableVat = product.Returnable?.Vat;
            ReturnableVatPrice = product.Returnable?.VatPrice;
            ReturnableWholeSalePrice = product.Returnable?.WholeSalePrice;
            ReturnableOnSalePrice = product.Returnable?.OnSalePrice;

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
        public decimal UnitVatPrice { get; private set; }
        public decimal UnitOnSalePrice { get; private set; }
        public decimal? UnitWeight { get; private set; }
        public decimal TotalProductWholeSalePrice { get; private set; }
        public decimal TotalProductVatPrice { get; private set; }
        public decimal TotalProductOnSalePrice { get; private set; }
        public decimal? TotalWeight { get; private set; }
        public bool HasReturnable { get; private set; }
        public Guid? ReturnableId { get; private set; }
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
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public Guid ProductId { get; private set; }
        public byte[] RowVersion { get; private set; }

        protected void RefreshLine()
        {
            TotalProductVatPrice = Math.Round(Quantity * UnitVatPrice, DIGITS_COUNT);
            TotalProductWholeSalePrice = Math.Round(Quantity * UnitWholeSalePrice, DIGITS_COUNT);
            TotalProductOnSalePrice = Math.Round(TotalProductVatPrice + TotalProductWholeSalePrice, DIGITS_COUNT);

            TotalReturnableVatPrice = HasReturnable ? Math.Round(Quantity * ReturnableVatPrice.Value, DIGITS_COUNT) : (decimal?)null;
            TotalReturnableWholeSalePrice = HasReturnable ? Math.Round(Quantity * ReturnableWholeSalePrice.Value, DIGITS_COUNT) : (decimal?)null;
            TotalReturnableOnSalePrice = HasReturnable ? Math.Round(TotalReturnableVatPrice.Value  + TotalReturnableWholeSalePrice.Value, DIGITS_COUNT) : (decimal?)null;

            TotalWeight = UnitWeight.HasValue ? Math.Round(Quantity * UnitWeight.Value, DIGITS_COUNT) : (decimal?)null;

            TotalVatPrice = Math.Round(TotalProductVatPrice + (TotalReturnableVatPrice ?? 0), DIGITS_COUNT);
            TotalWholeSalePrice = Math.Round(TotalProductWholeSalePrice + (TotalReturnableWholeSalePrice ?? 0), DIGITS_COUNT);
            TotalOnSalePrice = Math.Round(TotalVatPrice + TotalWholeSalePrice, DIGITS_COUNT);
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
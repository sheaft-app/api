using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class PurchaseOrderProduct
    {
        protected PurchaseOrderProduct()
        {
        }

        public PurchaseOrderProduct(Product product, CatalogProductPrice catalogProductPrice, int quantity)
        {
            Name = product.Name;
            Reference = product.Reference;
            Unit = product.Unit;
            Conditioning = product.Conditioning;
            QuantityPerUnit = product.QuantityPerUnit;
            WholeSalePrice = catalogProductPrice.WholeSalePrice;
            WholeSalePricePerUnit = catalogProductPrice.WholeSalePricePerUnit;
            UnitWeight = product.Weight;
            Vat = product.Vat;
            Quantity = quantity;
            ProductId = product.Id;
        }

        public string Name { get; private set; }
        public string Reference { get; private set; }
        public int Quantity { get; protected set; }
        public decimal Vat { get; private set; }
        public decimal WholeSalePrice { get; private set; }
        public decimal WholeSalePricePerUnit { get; private set; }
        public decimal UnitWeight { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public Guid ProductId { get; private set; }
        public PurchaseOrderProductReturnable Returnable { get; private set; }
    }

    public static class PurchaseOrderProductExtensions
    {
        
        public static string GetConditioning(this PurchaseOrderProduct productRow)
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
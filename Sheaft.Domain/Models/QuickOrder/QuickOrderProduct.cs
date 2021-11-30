using System;

namespace Sheaft.Domain
{
    public class QuickOrderProduct
    {
        protected QuickOrderProduct() { }

        public QuickOrderProduct(CatalogProductPrice catalogProductPrice, int? quantity = null)
        {
            CatalogId = catalogProductPrice.CatalogId;
            ProductId = catalogProductPrice.ProductId;
            Quantity = quantity;
        }
        
        public int? Quantity { get; private set; }
        public Guid CatalogId { get; private set; }
        public Guid ProductId { get; private set; }
    }
}
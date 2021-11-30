using Sheaft.Domain.Interop;
using System;

namespace Sheaft.Domain
{
    public class CartProduct : ITrackUpdate
    {
        protected CartProduct()
        {
        }

        public CartProduct(Guid catalogProductPriceId, int quantity)
        {
            Quantity = quantity;
            CatalogProductPriceId = catalogProductPriceId;
        }

        public int Quantity { get; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public Guid CatalogProductPriceId { get; }
        public Guid SupplierId { get; set; }
    }
}
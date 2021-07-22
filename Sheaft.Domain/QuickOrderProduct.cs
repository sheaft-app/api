using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class QuickOrderProduct: IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected QuickOrderProduct() { }

        public QuickOrderProduct(CatalogProduct catalogProduct, int? quantity = null)
        {
            CatalogProduct = catalogProduct;
            CatalogProductId = catalogProduct.Id;
            Quantity = quantity;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public int? Quantity { get; private set; }
        public Guid CatalogProductId { get; private set; }
        public Guid QuickOrderId { get; private set; }
        public virtual CatalogProduct CatalogProduct { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetQuantity(int? quantity)
        {
            if (quantity is < 0)
                throw SheaftException.Validation("La quantité du produit doit être supérieure ou égale à 0.");

            if (quantity == 0)
                quantity = null;

            Quantity = quantity;
        }
    }
}
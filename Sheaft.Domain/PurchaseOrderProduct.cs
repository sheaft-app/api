namespace Sheaft.Domain.Models
{
    public class PurchaseOrderProduct : ProductRow
    {
        protected PurchaseOrderProduct()
        {
        }

        public PurchaseOrderProduct(ProductRow product)
            : base(product)
        {
        }
    }
}
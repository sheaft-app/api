namespace Sheaft.Domain.Models
{
    public class PurchaseOrderProduct : ProductRow
    {
        protected PurchaseOrderProduct()
        {
        }

        public PurchaseOrderProduct(Product product, int quantity)
            : base(product, quantity)
        {
        }
        public PurchaseOrderProduct(ProductRow product)
            : base(product)
        {
        }
    }
}
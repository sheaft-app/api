using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class QuickOrderProduct
    {
        protected QuickOrderProduct() { }

        public QuickOrderProduct(Product product) : this(product, 0)
        {

        }

        public QuickOrderProduct(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public virtual Product Product { get; private set; }
        public int? Quantity { get; private set; }

        public void SetQuantity(int? quantity)
        {
            if (quantity.HasValue && quantity <= 0)
                throw new ValidationException(MessageKind.QuickOrder_ProductQuantity_CannotBe_LowerOrEqualThan, 0);

            Quantity = quantity;
        }
    }
}
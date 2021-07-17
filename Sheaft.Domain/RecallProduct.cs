using System;

namespace Sheaft.Domain
{
    public class RecallProduct : SurveillanceProduct
    {
        protected RecallProduct()
        {
        }

        public RecallProduct(Product product)
            : base(product)
        {
        }

        public Guid RecallId { get; private set; }
    }
}
using System;
using Sheaft.Domain.BaseClass;

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
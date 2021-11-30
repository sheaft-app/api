using System;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class ObservationProduct : SurveillanceProduct
    {
        protected ObservationProduct()
        {
        }

        public ObservationProduct(Product product)
            : base(product)
        {
        }

        public Guid ObservationId { get; private set; }
    }
}
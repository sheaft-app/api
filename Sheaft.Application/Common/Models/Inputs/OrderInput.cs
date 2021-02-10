using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class OrderInput
    {
        public DonationKind Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}
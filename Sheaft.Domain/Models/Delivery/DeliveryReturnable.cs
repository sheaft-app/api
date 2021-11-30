using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryReturnable
    {
        protected DeliveryReturnable()
        {
        }

        public DeliveryReturnable(Guid returnableId, int quantity)
        {
            ReturnableId = returnableId;
            PickedUpQuantity = quantity;
        }

        public int PickedUpQuantity { get; private set; }
        public Guid ReturnableId { get; private set; }
    }
}
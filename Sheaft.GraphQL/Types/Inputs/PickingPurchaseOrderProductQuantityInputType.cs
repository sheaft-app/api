using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PickingPurchaseOrderProductQuantityInputType : SheaftInputType<PickingPurchaseOrderProductQuantityDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PickingPurchaseOrderProductQuantityDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PickingPurchaseOrderProductQuantityInput");

            descriptor
                .Field(c => c.PurchaseOrderId)
                .ID(nameof(PurchaseOrder))
                .Name("purchaseOrderId");
            
            descriptor
                .Field(c => c.PreparedQuantity)
                .Name("preparedQuantity");
        }
    }
}
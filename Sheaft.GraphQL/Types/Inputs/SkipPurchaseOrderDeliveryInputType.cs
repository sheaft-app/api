using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrderDelivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SkipPurchaseOrderDeliveryInputType : SheaftInputType<SkipPurchaseOrderDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SkipPurchaseOrderDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SkipPurchaseOrderDeliveryInput");

            descriptor
                .Field(c => c.PurchaseOrderDeliveryId)
                .ID(nameof(PurchaseOrderDelivery))
                .Name("id");
        }
    }
}
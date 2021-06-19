using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrderDelivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class StartPurchaseOrderDeliveryInputType : SheaftInputType<StartPurchaseOrderDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<StartPurchaseOrderDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("StartPurchaseOrderDeliveryInput");

            descriptor
                .Field(c => c.PurchaseOrderDeliveryId)
                .ID(nameof(PurchaseOrderDelivery))
                .Name("id");
        }
    }
}
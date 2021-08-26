using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Billing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class MarkDeliveriesAsBilledInputType : SheaftInputType<MarkDeliveriesAsBilledCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MarkDeliveriesAsBilledCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("MarkDeliveriesAsBilledInput");

            descriptor
                .Field(c => c.DeliveryIds)
                .ID(nameof(Delivery))
                .Name("ids");
        }
    }
}
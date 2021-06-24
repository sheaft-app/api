using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RejectDeliveryInputType : SheaftInputType<RejectDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RejectDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RejectDeliveryInput");

            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(Delivery))
                .Name("id");

            descriptor
                .Field(c => c.Reason)
                .Name("reason");
        }
    }
}
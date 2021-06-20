using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class StartDeliveryInputType : SheaftInputType<StartDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<StartDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("StartDeliveryInput");

            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(Delivery))
                .Name("id");
        }
    }
}
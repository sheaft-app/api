using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetDeliveryModesAvailabilityInputType : SheaftInputType<SetDeliveryModesAvailabilityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetDeliveryModesAvailabilityCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetDeliveryModesAvailabilityInput");

            descriptor
                .Field(c => c.DeliveryModeIds)
                .Name("ids")
                .ID(nameof(DeliveryMode));

            descriptor
                .Field(c => c.Available)
                .Name("available");
        }
    }
}
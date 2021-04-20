using HotChocolate.Types;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetDeliveryModesAvailabilityInputType : SheaftInputType<SetDeliveryModesAvailabilityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetDeliveryModesAvailabilityCommand> descriptor)
        {
            descriptor.Name("SetDeliveryModesAvailabilityInput");
            descriptor.Field(c => c.DeliveryModeIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Available);
        }
    }
}
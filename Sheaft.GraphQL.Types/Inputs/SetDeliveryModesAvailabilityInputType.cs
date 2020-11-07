using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class SetDeliveryModesAvailabilityInputType : SheaftInputType<SetDeliveryModesAvailabilityInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetDeliveryModesAvailabilityInput> descriptor)
        {
            descriptor.Field(c => c.Available);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

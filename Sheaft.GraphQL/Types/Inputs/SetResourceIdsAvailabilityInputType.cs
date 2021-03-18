using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetResourceIdsAvailabilityInputType : SheaftInputType<SetResourceIdsAvailabilityDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetResourceIdsAvailabilityDto> descriptor)
        {
            descriptor.Name("SetResourceIdsAvailabilityInput");
            descriptor.Field(c => c.Available);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

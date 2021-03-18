using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetResourceIdsVisibilityInputType : SheaftInputType<SetResourceIdsVisibilityDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetResourceIdsVisibilityDto> descriptor)
        {
            descriptor.Name("SetResourceIdsVisibilityInput");
            descriptor.Field(c => c.VisibleToConsumers);
            descriptor.Field(c => c.VisibleToStores);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

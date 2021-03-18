using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdsWithReasonInputType : SheaftInputType<ResourceIdsWithReasonDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdsWithReasonDto> descriptor)
        {
            descriptor.Name("ResourceIdsWithReasonInput");
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

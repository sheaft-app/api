using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdsInputType : SheaftInputType<ResourceIdsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdsDto> descriptor)
        {
            descriptor.Name("ResourceIdsInput");
            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

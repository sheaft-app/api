using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdInputType : SheaftInputType<ResourceIdDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdDto> descriptor)
        {
            descriptor.Name("ResourceIdInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}

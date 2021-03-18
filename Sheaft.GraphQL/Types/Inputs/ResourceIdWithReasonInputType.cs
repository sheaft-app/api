using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdWithReasonInputType : SheaftInputType<ResourceIdWithReasonDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdWithReasonDto> descriptor)
        {
            descriptor.Name("ResourceIdWithReasonInput");
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}

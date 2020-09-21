using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class IdWithReasonInputType : SheaftInputType<IdWithReasonInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdWithReasonInput> descriptor)
        {
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}

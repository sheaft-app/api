using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class IdsWithReasonInputType : SheaftInputType<IdsWithReasonInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdsWithReasonInput> descriptor)
        {
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

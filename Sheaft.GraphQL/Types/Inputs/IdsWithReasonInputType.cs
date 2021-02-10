using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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

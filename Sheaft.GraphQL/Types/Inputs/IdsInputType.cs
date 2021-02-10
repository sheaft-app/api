using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class IdsInputType : SheaftInputType<IdsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdsInput> descriptor)
        {
            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

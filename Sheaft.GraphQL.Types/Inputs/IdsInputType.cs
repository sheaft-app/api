using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
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

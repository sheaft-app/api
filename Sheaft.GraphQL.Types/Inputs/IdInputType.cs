using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class IdInputType : SheaftInputType<IdInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}

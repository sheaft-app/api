using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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

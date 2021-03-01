using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateClosingInputType : SheaftInputType<UpdateClosingInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateClosingInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Reason);
        }
    }
}
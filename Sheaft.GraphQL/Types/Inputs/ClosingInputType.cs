using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ClosingInputType : SheaftInputType<ClosingInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ClosingInput> descriptor)
        {
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Reason);
        }
    }
}
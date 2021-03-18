using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ClosingInputType : SheaftInputType<CreateClosingDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateClosingDto> descriptor)
        {
            descriptor.Name("CreateClosingInput");
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Reason);
        }
    }
}
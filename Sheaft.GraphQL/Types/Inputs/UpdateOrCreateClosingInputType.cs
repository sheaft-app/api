using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateClosingInputType : SheaftInputType<UpdateOrCreateClosingDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateClosingDto> descriptor)
        {
            descriptor.Name("UpdateOrCreateClosingInput");
            descriptor.Field(c => c.Id).Type<IdType>();
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Reason);
        }
    }
}
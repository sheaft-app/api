using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateResourceIdClosingInputType : SheaftInputType<UpdateOrCreateResourceIdClosingDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateResourceIdClosingDto> descriptor)
        {
            descriptor.Name("UpdateOrCreateResourceIdClosingInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closing)
                .Type<NonNullType<UpdateOrCreateClosingInputType>>();
        }
    }
}
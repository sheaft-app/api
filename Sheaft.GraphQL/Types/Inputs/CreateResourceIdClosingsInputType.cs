using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateResourceIdClosingsInputType : SheaftInputType<CreateResourceIdClosingsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateResourceIdClosingsDto> descriptor)
        {
            descriptor.Name("CreateResourceIdClosingsInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<ClosingInputType>>>();
        }
    }
}
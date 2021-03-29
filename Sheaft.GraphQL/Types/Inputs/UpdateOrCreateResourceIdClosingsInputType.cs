using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateResourceIdClosingsInputType : SheaftInputType<UpdateOrCreateResourceIdClosingsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateResourceIdClosingsDto> descriptor)
        {
            descriptor.Name("UpdateOrCreateResourceIdClosingsInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<UpdateOrCreateClosingInputType>>>();
        }
    }
}
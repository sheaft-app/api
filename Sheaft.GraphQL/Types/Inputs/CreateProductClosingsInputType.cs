using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateProductClosingsInputType : SheaftInputType<CreateProductClosingsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductClosingsInput> descriptor)
        {
            descriptor.Field(c => c.ProductId)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<ClosingInputType>>>();
        }
    }
}
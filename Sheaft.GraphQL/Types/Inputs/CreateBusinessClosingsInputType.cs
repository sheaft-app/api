using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBusinessClosingsInputType : SheaftInputType<CreateBusinessClosingsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBusinessClosingsInput> descriptor)
        {
            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<ClosingInputType>>>();
        }
    }
}
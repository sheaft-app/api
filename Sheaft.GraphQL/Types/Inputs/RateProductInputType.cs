using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RateProductInputType : SheaftInputType<RateProductInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RateProductInput> descriptor)
        {
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}

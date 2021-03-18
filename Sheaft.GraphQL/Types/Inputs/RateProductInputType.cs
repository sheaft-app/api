using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RateProductInputType : SheaftInputType<RateProductDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RateProductDto> descriptor)
        {
            descriptor.Name("RateProductInput");
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}

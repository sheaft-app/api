using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RateProductInputType : SheaftInputType<RateProductCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RateProductCommand> descriptor)
        {
            descriptor.Name("RateProductInput");
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.ProductId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}

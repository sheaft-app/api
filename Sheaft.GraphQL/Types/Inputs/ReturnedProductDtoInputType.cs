using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ReturnedProductDtoInputType : SheaftInputType<ReturnedProductDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ReturnedProductDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ReturnedProductInput");

            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));

            descriptor
                .Field(c => c.Kind)
                .Name("kind");

            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");
        }
    }
}
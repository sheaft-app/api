using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetProductsAvailabilityInputType : SheaftInputType<SetProductsAvailabilityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetProductsAvailabilityCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetProductsAvailabilityInput");

            descriptor
                .Field(c => c.Available)
                .Name("available");

            descriptor
                .Field(c => c.ProductIds)
                .Name("ids")
                .ID(nameof(Product));
        }
    }
}

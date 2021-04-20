using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetProductsAvailabilityInputType : SheaftInputType<SetProductsAvailabilityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetProductsAvailabilityCommand> descriptor)
        {
            descriptor.Name("SetProductsAvailabilityInput");
            descriptor.Field(c => c.Available);

            descriptor.Field(c => c.ProductIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

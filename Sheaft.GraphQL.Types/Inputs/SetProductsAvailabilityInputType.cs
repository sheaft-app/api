using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class SetProductsAvailabilityInputType : SheaftInputType<SetProductsAvailabilityInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetProductsAvailabilityInput> descriptor)
        {
            descriptor.Field(c => c.Available);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}

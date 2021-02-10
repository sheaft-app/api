using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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

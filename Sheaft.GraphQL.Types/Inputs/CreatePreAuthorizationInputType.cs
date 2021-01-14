using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreatePreAuthorizationInputType : SheaftInputType<CreatePreAuthorizationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePreAuthorizationInput> descriptor)
        {
            descriptor.Field(c => c.OrderId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.CardIdentifier)
                .Type<NonNullType<StringType>>();
        }
    }
}

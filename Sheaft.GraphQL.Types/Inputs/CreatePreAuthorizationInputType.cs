using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreatePreAuthorizationInputType : SheaftInputType<CreatePreAuthorizationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePreAuthorizationInput> descriptor)
        {
            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.OrderId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.CardId)
                .Type<NonNullType<StringType>>();
        }
    }
}

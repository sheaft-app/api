using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.PreAuthorization.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreatePreAuthorizationInputType : SheaftInputType<CreatePreAuthorizationForOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePreAuthorizationForOrderCommand> descriptor)
        {
            descriptor.Name("CreatePreAuthorizationInput");
            descriptor.Field(c => c.OrderId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.CardIdentifier)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.BrowserInfo)
                .Type<NonNullType<BrowserInfoInputType>>();
        }
    }
}
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.PreAuthorization.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreatePreAuthorizationForOrderInputType : SheaftInputType<CreatePreAuthorizationForOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePreAuthorizationForOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreatePreAuthorizationForOrderInput");

            descriptor
                .Field(c => c.OrderId)
                .Name("id")
                .ID(nameof(Order));

            descriptor
                .Field(c => c.CardIdentifier)
                .Name("cardIdentifier")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.BrowserInfo)
                .Name("browserInfo")
                .Type<NonNullType<BrowserInfoInputType>>();
        }
    }
}
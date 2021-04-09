using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreatePreAuthorizationInputType : SheaftInputType<CreatePreAuthorizationDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePreAuthorizationDto> descriptor)
        {
            descriptor.Name("CreatePreAuthorizationInput");
            descriptor.Field(c => c.OrderId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.CardIdentifier)
                .Type<NonNullType<StringType>>();
        }
    }
}
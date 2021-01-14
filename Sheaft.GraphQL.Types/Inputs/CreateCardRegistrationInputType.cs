using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreateCardRegistrationInputType : SheaftInputType<CreateCardRegistrationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCardRegistrationInput> descriptor)
        {
            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();
        }
    }
}

using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ValidateCardRegistrationInputType : SheaftInputType<ValidateCardRegistrationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ValidateCardRegistrationInput> descriptor)
        {
            descriptor.Field(c => c.CardId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.RegistrationData);
            descriptor.Field(c => c.Remember);
        }
    }
}

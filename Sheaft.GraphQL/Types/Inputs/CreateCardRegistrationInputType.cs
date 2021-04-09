using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateCardRegistrationInputType : SheaftInputType<CreateCardRegistrationDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCardRegistrationDto> descriptor)
        {
            descriptor.Name("CreateCardRegistrationInput");
            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();
        }
    }
}
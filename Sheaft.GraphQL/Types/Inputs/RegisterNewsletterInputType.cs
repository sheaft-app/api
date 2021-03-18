using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterNewsletterInputType : SheaftInputType<RegisterNewsletterDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterNewsletterDto> descriptor)
        {
            descriptor.Name("RegisterNewsletterInput");
            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Role)
                .Type<NonNullType<StringType>>();
        }
    }
}

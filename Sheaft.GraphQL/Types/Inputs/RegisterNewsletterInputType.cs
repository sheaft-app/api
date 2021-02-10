using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterNewsletterInputType : SheaftInputType<RegisterNewsletterInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterNewsletterInput> descriptor)
        {
            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Role)
                .Type<NonNullType<StringType>>();
        }
    }
}

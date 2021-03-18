using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterConsumerInputType : SheaftInputType<RegisterConsumerDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterConsumerDto> descriptor)
        {
            descriptor.Name("RegisterConsumerInput");
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);
            descriptor.Field(c => c.Summary);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Facebook);
            descriptor.Field(c => c.Twitter);
            descriptor.Field(c => c.Instagram);
            descriptor.Field(c => c.Website);
            descriptor.Field(c => c.Anonymous);

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.Address)
                .Type<AddressInputType>();
        }
    }
}

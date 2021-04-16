using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Application.Security;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class UserType : SheaftOutputType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.Phone)
                .Authorize(Policies.REGISTERED);

            descriptor.Field(c => c.Email)
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();
            
            descriptor.Field(c => c.Summary);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Facebook);
            descriptor.Field(c => c.Twitter);
            descriptor.Field(c => c.Instagram);
            descriptor.Field(c => c.Website);
        }
    }
}

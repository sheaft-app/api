using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UserProfileType : SheaftOutputType<UserProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserProfileDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();
        }
    }
}

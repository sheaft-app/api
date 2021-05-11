using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.GraphQL.Users;
using Sheaft.Options;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class UserType : SheaftOutputType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<UsersByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field("profileId")
                .ResolveWith<UserResolvers>(c => c.GetUserProfileId(default, default, default, default))
                .Type<IdType>();
            
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
                
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.FirstName)
                .Name("firstName")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.LastName)
                .Name("lastName")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<UserAddressType>>();
        }

        private class UserResolvers
        {
            public string GetUserProfileId(User user, [Service] IOptionsSnapshot<RoleOptions> roleOptions,[Service] ICurrentUserService currentUserService, [Service] IIdSerializer serializer)
            {
                var userResult = currentUserService.GetCurrentUserInfo();
                if (!userResult.Succeeded)
                    return null;

                if (userResult.Data.IsInRole(roleOptions.Value.Producer.Value))
                    return serializer.Serialize("Query", nameof(Producer), user.Id);
                
                if (userResult.Data.IsInRole(roleOptions.Value.Store.Value))
                    return serializer.Serialize("Query", nameof(Store), user.Id);
                
                if (userResult.Data.IsInRole(roleOptions.Value.Consumer.Value))
                    return serializer.Serialize("Query", nameof(Consumer), user.Id);

                return null;
            }
        }
    }
}

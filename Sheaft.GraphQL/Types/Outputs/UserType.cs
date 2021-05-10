using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;

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
    }
}

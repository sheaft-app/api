using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Consumers;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ConsumerType : SheaftOutputType<Consumer>
    {
        protected override void Configure(IObjectTypeDescriptor<Consumer> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<ConsumersByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Authorize(Policies.REGISTERED)
                .Type<UserAddressType>();

            descriptor
                .Field(c => c.FirstName)
                .Name("firstName")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.LastName)
                .Name("lastName")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Phone)
                .Name("phone")
                .Authorize(Policies.REGISTERED);
            
            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Authorize(Policies.REGISTERED)
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.Anonymous)
                .Name("anonymous");
                
            descriptor
                .Field(c => c.Summary)
                .Name("summary");
                
            descriptor
                .Field(c => c.Description)
                .Name("description");
                
            descriptor
                .Field(c => c.Facebook)
                .Name("facebook");
                
            descriptor
                .Field(c => c.Twitter)
                .Name("twitter");
                
            descriptor
                .Field(c => c.Instagram)
                .Name("instagram");
                
            descriptor
                .Field(c => c.Website)
                .Name("website");
        }
    }
}

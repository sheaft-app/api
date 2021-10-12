using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Consumers;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ConsumerProfileType : SheaftOutputType<Consumer>
    {
        protected override void Configure(IObjectTypeDescriptor<Consumer> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ConsumerProfile");
            
            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(Consumer));
            
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
                .Name("phone");
            
            descriptor
                .Field(c => c.Email)
                .Name("email")
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
                
            descriptor
                .Field(c => c.Legal)
                .Name("legals")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ConsumerProfileResolvers>(c => c.GetLegals(default!, default!, default!, default))
                .Type<ConsumerLegalType>();
        }

        private class ConsumerProfileResolvers
        {
            public async Task<ConsumerLegal> GetLegals(Consumer consumer,
                [ScopedService] QueryDbContext context,
                ConsumerLegalsByIdBatchDataLoader consumerLegalsDataLoader, CancellationToken token)
            {
                var legalId = await context.Legals.OfType<ConsumerLegal>()
                    .Where(p => p.UserId == consumer.Id)
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync(token);

                return await consumerLegalsDataLoader.LoadAsync(legalId, token);
            }
        }
    }
}

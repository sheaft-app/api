using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Agreements;
using Sheaft.GraphQL.Business;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducerProfileType : SheaftOutputType<Producer>
    {
        protected override void Configure(IObjectTypeDescriptor<Producer> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<ProducersByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Picture)
                .Name("picture");

            descriptor
                .Field(c => c.NotSubjectToVat)
                .Name("notSubjectToVat");

            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");

            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field(c => c.Phone)
                .Name("phone");

            descriptor
                .Field(c => c.Email)
                .Name("email")
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
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

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
                .Field(c => c.OpenForNewBusiness)
                .Name("openForNewBusiness");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<UserAddressType>>();
            
            descriptor
                .Field(c => c.Legal)
                .Name("legals")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerProfileResolvers>(c => c.GetLegals(default!, default!, default!, default))
                .Type<ListType<BusinessLegalType>>();
            
            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerProfileResolvers>(c => c.GetClosings(default!, default!, default!, default))
                .Type<ListType<BusinessClosingType>>();

            descriptor
                .Field(c => c.Pictures)
                .Name("pictures")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerProfileResolvers>(c => c.GetPictures(default!, default!, default!, default))
                .Type<ListType<ProfilePictureType>>();
        }

        private class ProducerProfileResolvers
        {
            public async Task<BusinessLegal> GetLegals(Producer producer,
                [ScopedService] QueryDbContext context,
                BusinessLegalsByIdBatchDataLoader businessLegalsDataLoader, CancellationToken token)
            {
                var legalId = await context.Legals.OfType<BusinessLegal>()
                    .Where(p => p.UserId == producer.Id)
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync(token);

                return await businessLegalsDataLoader.LoadAsync(legalId, token);
            }
            
            public async Task<IEnumerable<BusinessClosing>> GetClosings(Producer producer,
                [ScopedService] QueryDbContext context,
                BusinessClosingsByIdBatchDataLoader closingsDataLoader, CancellationToken token)
            {
                var closingsId = await context.Set<BusinessClosing>()
                    .Where(p => p.BusinessId == producer.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await closingsDataLoader.LoadAsync(closingsId, token);
            }

            public async Task<IEnumerable<ProfilePicture>> GetPictures(Producer producer,
                [ScopedService] QueryDbContext context,
                ProfilePicturesByIdBatchDataLoader picturesDataLoader, CancellationToken token)
            {
                var picturesId = await context.Set<ProfilePicture>()
                    .Where(p => p.UserId == producer.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await picturesDataLoader.LoadAsync(picturesId, token);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Agreements;
using Sheaft.GraphQL.Business;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Stores;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class StoreProfileType : SheaftOutputType<Store>
    {
        protected override void Configure(IObjectTypeDescriptor<Store> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("StoreProfile");
            
            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(Store));

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
                .Field(c => c.Tags)
                .Name("tags")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreProfileResolvers>(c => c.GetTags(default!, default!, default!, default))
                .Type<ListType<TagType>>();

            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreProfileResolvers>(c => c.GetClosings(default!, default!, default!, default))
                .Type<ListType<BusinessClosingType>>();
            
            descriptor
                .Field(c => c.Closings)
                .Name("openingHours")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreProfileResolvers>(c => c.GetOpeningHours(default!, default!, default!, default))
                .Type<ListType<OpeningHoursType>>();

            descriptor
                .Field(c => c.Pictures)
                .Name("pictures")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreProfileResolvers>(c => c.GetPictures(default!, default!, default!, default))
                .Type<ListType<ProfilePictureType>>();
            
            descriptor
                .Field(c => c.Legal)
                .Name("legals")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreProfileResolvers>(c => c.GetLegals(default!, default!, default!, default))
                .Type<ListType<BusinessLegalType>>();
        }

        private class StoreProfileResolvers
        {
            public async Task<BusinessLegal> GetLegals(Store store,
                [ScopedService] QueryDbContext context,
                BusinessLegalsByIdBatchDataLoader businessLegalsDataLoader, CancellationToken token)
            {
                var legalId = await context.Legals.OfType<BusinessLegal>()
                    .Where(p => p.UserId == store.Id)
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync(token);

                return await businessLegalsDataLoader.LoadAsync(legalId, token);
            }
            
            public async Task<IEnumerable<Tag>> GetTags(Store store, [ScopedService] QueryDbContext context,
                TagsByIdBatchDataLoader tagsDataLoader, CancellationToken token)
            {
                var tagsId = await context.Set<StoreTag>()
                    .Where(p => p.StoreId == store.Id)
                    .Select(p => p.TagId)
                    .ToListAsync(token);

                return await tagsDataLoader.LoadAsync(tagsId, token);
            }

            public async Task<IEnumerable<BusinessClosing>> GetClosings(Store store, [ScopedService] QueryDbContext context,
                BusinessClosingsByIdBatchDataLoader closingsDataLoader, CancellationToken token)
            {
                var closingsId = await context.Set<BusinessClosing>()
                    .Where(p => p.BusinessId == store.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await closingsDataLoader.LoadAsync(closingsId, token);
            }

            public async Task<IEnumerable<Domain.OpeningHours>> GetOpeningHours(Store store, [ScopedService] QueryDbContext context,
                OpeningHoursByIdBatchDataLoader openingHoursDataLoader, CancellationToken token)
            {
                var openingHoursId = await context.Set<Domain.OpeningHours>()
                    .Where(p => p.StoreId == store.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await openingHoursDataLoader.LoadAsync(openingHoursId, token);
            }

            public async Task<IEnumerable<ProfilePicture>> GetPictures(Store store, [ScopedService] QueryDbContext context,
                ProfilePicturesByIdBatchDataLoader picturesDataLoader, CancellationToken token)
            {
                var picturesId = await context.Set<ProfilePicture>()
                    .Where(p => p.UserId == store.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await picturesDataLoader.LoadAsync(picturesId, token);
            }
        }
    }
}

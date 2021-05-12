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
    public class StoreType : SheaftOutputType<Store>
    {
        protected override void Configure(IObjectTypeDescriptor<Store> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<StoresByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

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
                .Name("phone")
                .Authorize(Policies.REGISTERED);

            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Authorize(Policies.REGISTERED)
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
                .Field(c => c.ClosingsCount)
                .Name("closingsCount");

            descriptor
                .Field(c => c.TagsCount)
                .Name("tagsCount");

            descriptor
                .Field(c => c.PicturesCount)
                .Name("picturesCount");

            descriptor
                .Field(c => c.OpeningHoursCount)
                .Name("openingHoursCount");
            
            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<UserAddressType>>();
            
            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreResolvers>(c => c.GetTags(default!, default!, default!, default))
                .Type<ListType<TagType>>();

            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreResolvers>(c => c.GetClosings(default!, default!, default!, default))
                .Type<ListType<BusinessClosingType>>();
            
            descriptor
                .Field(c => c.OpeningHours)
                .Name("openingHours")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreResolvers>(c => c.GetOpeningHours(default!, default!, default!, default))
                .Type<ListType<OpeningHoursType>>();

            descriptor
                .Field(c => c.Pictures)
                .Name("pictures")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreResolvers>(c => c.GetPictures(default!, default!, default!, default))
                .Type<ListType<ProfilePictureType>>();
            
            descriptor
                .Field("agreement")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<StoreResolvers>(c => c.GetCurrentAgreement(default!, default!,  default!, default!, default))
                .Type<AgreementType>();
        }

        private class StoreResolvers
        {
            public async Task<Agreement> GetCurrentAgreement(Store store, 
                [ScopedService] QueryDbContext context, [Service] ICurrentUserService currentUserService,
                AgreementsByIdBatchDataLoader agreementsDataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;
                
                var agreementId = await context.Agreements
                    .Where(p => p.StoreId == store.Id && p.ProducerId == currentUser.Data.Id && p.Status != AgreementStatus.Cancelled && p.Status != AgreementStatus.Refused)
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync(token);

                if (agreementId == Guid.Empty)
                    return null;

                return await agreementsDataLoader.LoadAsync(agreementId, token);
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

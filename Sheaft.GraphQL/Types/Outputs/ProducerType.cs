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
using Sheaft.GraphQL.Recalls;
using Sheaft.GraphQL.Stores;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducerType : SheaftOutputType<Producer>
    {
        protected override void Configure(IObjectTypeDescriptor<Producer> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("Producer");
            
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
                .Field(c => c.ClosingsCount)
                .Name("closingsCount");

            descriptor
                .Field(c => c.TagsCount)
                .Name("tagsCount");

            descriptor
                .Field(c => c.PicturesCount)
                .Name("picturesCount");

            descriptor
                .Field("productsCount")
                .ResolveWith<ProducerResolvers>(c => c.GetProductsCount(default!, default!, default, default, default));

            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<UserAddressType>>();

            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c => c.GetTags(default!, default!, default!, default))
                .Type<ListType<TagType>>();

            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c => c.GetClosings(default!, default!, default!, default))
                .Type<ListType<BusinessClosingType>>();

            descriptor
                .Field(c => c.Pictures)
                .Name("pictures")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c => c.GetPictures(default!, default!, default!, default))
                .Type<ListType<ProfilePictureType>>();

            descriptor
                .Field("agreement")
                .Authorize(Policies.STORE_OR_PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c =>
                    c.GetCurrentAgreement(default!, default!, default!, default!, default))
                .Type<AgreementType>();

            descriptor
                .Field("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c =>
                    c.GetProducts(default!, default!, default!, default, default!, default))
                .Type<ListType<ProductType>>();
            
            descriptor
                .Field("stores")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c =>
                    c.GetStores(default!, default!, default!, default))
                .Type<ListType<StoreType>>();
                
            descriptor
                .Field("recalls")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProducerResolvers>(c =>
                    c.GetRecalls(default!, default!, default!, default))
                .Type<ListType<RecallType>>();
        }

        private class ProducerResolvers
        {
            public async Task<IEnumerable<Product>> GetProducts(Producer producer,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                var productsId = new List<Guid>();
                if (currentUser.Data.IsInRole(roleOptions.Value.Store.Value))
                {
                    var hasAgreement = await context.Agreements
                        .Where(c => c.StoreId == currentUser.Data.Id && c.ProducerId == producer.Id &&
                                    c.Status == AgreementStatus.Accepted)
                        .AnyAsync(token);

                    if (hasAgreement)
                        productsId = await context.Agreements
                            .Where(c => c.StoreId == currentUser.Data.Id && c.ProducerId == producer.Id &&
                                        c.Catalog.Kind == CatalogKind.Stores && c.Status == AgreementStatus.Accepted)
                            .SelectMany(a => a.Catalog.Products)
                            .Where(c => !c.Product.RemovedOn.HasValue)
                            .Select(c => c.ProductId)
                            .ToListAsync(token);
                    else
                        productsId = await context.Products
                            .Where(p => p.ProducerId == producer.Id && p.CatalogsPrices.Any(cp =>
                                cp.Catalog.Kind == CatalogKind.Stores && cp.Catalog.Available && cp.Catalog.IsDefault))
                            .Select(p => p.Id)
                            .ToListAsync(token);
                }
                else
                {
                    productsId = await context.Products
                        .Where(p => p.ProducerId == producer.Id && p.CatalogsPrices.Any(cp =>
                            cp.Catalog.Kind == CatalogKind.Consumers && cp.Catalog.Available))
                        .Select(p => p.Id)
                        .ToListAsync(token);
                }

                return await productsDataLoader.LoadAsync(productsId, token);
            }
            
            public async Task<IEnumerable<Store>> GetStores(Producer producer,
                [ScopedService] QueryDbContext context,
                StoresByIdBatchDataLoader storesDataLoader, CancellationToken token)
            {
                var storeIds = await context.Agreements
                    .Where(c => c.ProducerId == producer.Id &&
                                c.Status == AgreementStatus.Accepted)
                    .Select(a => a.StoreId)
                    .ToListAsync(token);

                return await storesDataLoader.LoadAsync(storeIds, token);
            }
            
            public async Task<IEnumerable<Recall>> GetRecalls(Producer producer,
                [ScopedService] QueryDbContext context,
                RecallsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var recallIds = await context.Recalls
                    .Where(c => c.ProducerId == producer.Id && (int)c.Status >= (int)RecallStatus.Ready)
                    .Select(a => a.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(recallIds, token);
            }

            public async Task<int> GetProductsCount(Producer producer,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return 0;

                if (currentUser.Data.IsInRole(roleOptions.Value.Store.Value))
                {
                    var hasAgreement = await context.Agreements
                        .Where(c => c.StoreId == currentUser.Data.Id && c.ProducerId == producer.Id &&
                                    c.Status == AgreementStatus.Accepted)
                        .AnyAsync(token);

                    if (hasAgreement)
                        return await context.Agreements
                            .Where(c => c.StoreId == currentUser.Data.Id && c.ProducerId == producer.Id &&
                                        c.Catalog.Kind == CatalogKind.Stores && c.Status == AgreementStatus.Accepted)
                            .SelectMany(a => a.Catalog.Products)
                            .Where(c => !c.Product.RemovedOn.HasValue)
                            .CountAsync(token);

                    return await context.Products
                        .CountAsync(p => p.ProducerId == producer.Id && p.CatalogsPrices.Any(cp =>
                                cp.Catalog.Kind == CatalogKind.Stores && cp.Catalog.Available && cp.Catalog.IsDefault),
                            token);
                }

                return await context.Products
                    .CountAsync(
                        p => p.ProducerId == producer.Id && p.CatalogsPrices.Any(cp =>
                            cp.Catalog.Kind == CatalogKind.Consumers && cp.Catalog.Available), token);
            }

            public async Task<Agreement> GetCurrentAgreement(Producer producer,
                [ScopedService] QueryDbContext context, [Service] ICurrentUserService currentUserService,
                AgreementsByIdBatchDataLoader agreementsDataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                var agreementId = await context.Agreements
                    .Where(p => p.ProducerId == producer.Id && p.StoreId == currentUser.Data.Id &&
                                p.Status != AgreementStatus.Cancelled && p.Status != AgreementStatus.Refused)
                    .Select(p => p.Id)
                    .SingleOrDefaultAsync(token);

                if (agreementId == Guid.Empty)
                    return null;

                return await agreementsDataLoader.LoadAsync(agreementId, token);
            }

            public async Task<IEnumerable<Tag>> GetTags(Producer producer, [ScopedService] QueryDbContext context,
                TagsByIdBatchDataLoader tagsDataLoader, CancellationToken token)
            {
                var tagsId = await context.Set<ProducerTag>()
                    .Where(p => p.ProducerId == producer.Id)
                    .Select(p => p.TagId)
                    .ToListAsync(token);

                return await tagsDataLoader.LoadAsync(tagsId, token);
            }

            public async Task<IEnumerable<BusinessClosing>> GetClosings(Producer producer,
                [ScopedService] QueryDbContext context,
                BusinessClosingsByIdBatchDataLoader closingsDataLoader, CancellationToken token)
            {
                var closingsId = await context.Set<BusinessClosing>()
                    .Where(p => p.BusinessId == producer.Id && p.ClosedTo > DateTimeOffset.UtcNow)
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
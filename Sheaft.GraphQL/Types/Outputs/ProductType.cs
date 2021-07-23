using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Observations;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.Ratings;
using Sheaft.GraphQL.Recalls;
using Sheaft.GraphQL.Returnables;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProductType : SheaftOutputType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<ProductsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Reference)
                .Name("reference")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");

            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field(c => c.Vat)
                .Name("vat");

            descriptor
                .Field(c => c.Available)
                .Name("available");

            descriptor
                .Field(c => c.RatingsCount)
                .Name("ratingsCount");
            
            descriptor
                .Field(c => c.PicturesCount)
                .Name("picturesCount");
            
            descriptor
                .Field(c => c.CatalogsPricesCount)
                .Name("catalogsCount")
                .Authorize(Policies.PRODUCER);

            descriptor
                .Field(c => c.QuantityPerUnit)
                .Name("quantityPerUnit");

            descriptor
                .Field(c => c.Unit)
                .Name("unit");

            descriptor
                .Field(c => c.Conditioning)
                .Name("conditioning");

            descriptor
                .Field(c => c.Description)
                .Name("description");

            descriptor
                .Field(c => c.Weight)
                .Name("weight");

            descriptor
                .Field(c => c.Rating)
                .Name("rating");

            descriptor
                .Field(c => c.Picture)
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture,
                        PictureSize.MEDIUM))
                .Name("picture");

            descriptor
                .Field("imageLarge")
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture,
                        PictureSize.LARGE))
                .Type<StringType>();

            descriptor
                .Field("imageMedium")
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture,
                        PictureSize.MEDIUM))
                .Type<StringType>();

            descriptor
                .Field("imageSmall")
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture,
                        PictureSize.SMALL))
                .Type<StringType>();

            descriptor
                .Field("isReturnable")
                .Resolve(c => c.Parent<Product>().ReturnableId.HasValue)
                .Type<BooleanType>();

            descriptor
                .Field("visibleTo")
                .Resolve(c => c.Parent<Product>().VisibleTo)
                .Authorize(Policies.PRODUCER);

            descriptor
                .Field("vatPricePerUnit")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductVatPricePerUnit(default, default, default, default, default, default))
                .Type<DecimalType>();

            descriptor
                .Field("onSalePricePerUnit")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductOnSalePricePerUnit(default, default, default, default, default, default))
                .Type<DecimalType>();

            descriptor
                .Field("wholeSalePricePerUnit")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c =>
                    c.GetProductWholeSalePricePerUnit(default, default, default, default, default, default))
                .Type<DecimalType>();

            descriptor
                .Field("onSalePrice")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductOnSalePrice(default, default, default, default, default, default))
                .Type<DecimalType>();

            descriptor
                .Field("wholeSalePrice")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductWholeSalePrice(default, default, default, default, default, default))
                .Type<DecimalType>();

            descriptor
                .Field("vatPrice")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductVatPrice(default, default, default, default,  default, default))
                .Type<DecimalType>();

            descriptor
                .Field(c => c.CatalogsPrices)
                .Name("catalogs")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductCatalogs(default, default, default, default))
                .Type<ListType<CatalogPriceType>>()
                .Authorize(Policies.PRODUCER);

            descriptor
                .Field("currentUserHasRatedProduct")
                .Type<BooleanType>()
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.ProductIsRatedByUser(default, default, default!, default));

            descriptor
                .Field(c => c.Returnable)
                .Name("returnable")
                .ResolveWith<ProductResolvers>(c => c.GetReturnable(default!, default!, default))
                .Type<ReturnableType>();

            descriptor
                .Field(c => c.Producer)
                .Name("producer")
                .ResolveWith<ProductResolvers>(c => c.GetProducer(default!, default!, default))
                .Type<ProducerType>();

            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetTags(default!, default!, default!, default))
                .Type<ListType<TagType>>();
            
            descriptor
                .Field(c => c.Ratings)
                .Name("ratings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetRatings(default!, default!, default, default))
                .Type<ListType<RatingType>>()
                .UsePaging()
                .UseSorting();

            descriptor
                .Field(c => c.Pictures)
                .Name("pictures")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetPictures(default!, default!, default, default))
                .Type<ListType<ProductPictureType>>();
            
            descriptor
                .Field("observations")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetObservations(default, default, default, default, default, default))
                .Type<ListType<ObservationType>>();
            
            descriptor
                .Field("recalls")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<ProductResolvers>(c =>
                    c.GetRecalls(default!, default!, default!, default))
                .Type<ListType<RecallType>>();
        }

        private class ProductResolvers
        {
            public async Task<IEnumerable<Recall>> GetRecalls(Product product,
                [ScopedService] QueryDbContext context,
                RecallsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var recallIds = await context.Recalls
                    .Where(c => c.Products.Any(p => p.ProductId == product.Id) && (int)c.Status >= (int)RecallStatus.Ready)
                    .Select(a => a.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(recallIds, token);
            }
            
            public async Task<IEnumerable<Observation>> GetObservations(Product product,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                ObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                var observationIds = new List<Guid>();
                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                {
                    observationIds = await context.Observations
                        .Where(cp => cp.ProducerId == currentUser.Data.Id && !cp.ReplyToId.HasValue && cp.Products.Any(b => b.ProductId == product.Id))
                        .Select(cp => cp.Id)
                        .ToListAsync(token);
                }
                else
                {
                    observationIds = await context.Observations
                        .Where(cp => (cp.VisibleToAll || cp.UserId == currentUser.Data.Id) && !cp.ReplyToId.HasValue && cp.Products.Any(b => b.ProductId == product.Id))
                        .Select(cp => cp.Id)
                        .ToListAsync(token);
                }

                var result = await dataLoader.LoadAsync(observationIds.Distinct().ToList(), token);
                return result.OrderBy(o => o.CreatedOn);
            }
            
            public Task<bool> ProductIsRatedByUser(Product product, [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService, CancellationToken token)
            {
                var currentUserResult = currentUserService.GetCurrentUserInfo();
                if (!currentUserResult.Succeeded)
                    return null;

                return context.Set<Rating>()
                    .AnyAsync(r => r.ProductId == product.Id && r.UserId == currentUserResult.Data.Id, token);
            }

            public async Task<IEnumerable<ProductPicture>> GetPictures(Product product,
                [ScopedService] QueryDbContext context,
                ProductPicturesByIdBatchDataLoader picturesDataLoader, CancellationToken token)
            {
                var picturesId = await context.Set<ProductPicture>()
                    .Where(p => p.ProductId == product.Id)
                    .OrderBy(p => p.Position)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await picturesDataLoader.LoadAsync(picturesId, token);
            }

            public async Task<IEnumerable<CatalogProduct>> GetProductCatalogs(Product product,
                [ScopedService] QueryDbContext context, CatalogProductsByIdBatchDataLoader catalogProductsDataLoader,
                CancellationToken token)
            {
                var catalogProductsId = await context.Set<CatalogProduct>()
                    .Where(cp => cp.ProductId == product.Id && !cp.Catalog.RemovedOn.HasValue)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await catalogProductsDataLoader.LoadAsync(catalogProductsId, token);
            }

            public async Task<IEnumerable<Tag>> GetTags(Product product, [ScopedService] QueryDbContext context,
                TagsByIdBatchDataLoader tagsDataLoader, CancellationToken token)
            {
                var tagsId = await context.Set<ProductTag>()
                    .Where(p => p.ProductId == product.Id && !p.Tag.RemovedOn.HasValue)
                    .Select(p => p.TagId)
                    .ToListAsync(token);

                return await tagsDataLoader.LoadAsync(tagsId, token);
            }

            public async Task<IEnumerable<Rating>> GetRatings(Product product, [ScopedService] QueryDbContext context,
                RatingsByIdBatchDataLoader ratingsDataLoader, CancellationToken token)
            {
                var ratingsId = await context.Set<Rating>()
                    .Where(p => p.ProductId == product.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await ratingsDataLoader.LoadAsync(ratingsId, token);
            }

            public Task<Producer> GetProducer(Product product, ProducersByIdBatchDataLoader producersDataLoader,
                CancellationToken token)
            {
                return producersDataLoader.LoadAsync(product.ProducerId, token);
            }

            public Task<Returnable> GetReturnable(Product product, ReturnablesByIdBatchDataLoader returnablesDataLoader,
                CancellationToken token)
            {
                if (!product.ReturnableId.HasValue)
                    return null;

                return returnablesDataLoader.LoadAsync(product.ReturnableId.Value, token);
            }

            public async Task<decimal> GetProductVatPricePerUnit(Product product, 
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CatalogProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var catalogProductId =
                    await GetCatalogProductId(product, context, currentUserService, roleOptions, token);

                if (!catalogProductId.HasValue)
                    return 0m;

                var catalogProduct = await dataLoader.LoadAsync(catalogProductId.Value, token);
                return catalogProduct.VatPricePerUnit;
            }

            public async Task<decimal> GetProductWholeSalePricePerUnit(Product product, 
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CatalogProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var catalogProductId =
                    await GetCatalogProductId(product, context, currentUserService, roleOptions, token);

                if (!catalogProductId.HasValue)
                    return 0m;

                var catalogProduct = await dataLoader.LoadAsync(catalogProductId.Value, token);
                return catalogProduct.WholeSalePricePerUnit;
            }

            public async Task<decimal> GetProductOnSalePricePerUnit(Product product, 
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CatalogProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var catalogProductId =
                    await GetCatalogProductId(product, context, currentUserService, roleOptions, token);

                if (!catalogProductId.HasValue)
                    return 0m;
                
                var catalogProduct = await dataLoader.LoadAsync(catalogProductId.Value, token);
                return catalogProduct.OnSalePricePerUnit;
            }

            public async Task<decimal> GetProductVatPrice(Product product,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CatalogProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var catalogProductId =
                    await GetCatalogProductId(product, context, currentUserService, roleOptions, token);

                if (!catalogProductId.HasValue)
                    return 0m;

                var catalogProduct = await dataLoader.LoadAsync(catalogProductId.Value, token);
                return catalogProduct.VatPrice;
            }

            public async Task<decimal> GetProductWholeSalePrice(Product product, 
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CatalogProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var catalogProductId =
                    await GetCatalogProductId(product, context, currentUserService, roleOptions, token);

                if (!catalogProductId.HasValue)
                    return 0m;

                var catalogProduct = await dataLoader.LoadAsync(catalogProductId.Value, token);
                return catalogProduct.WholeSalePrice;
            }

            public async Task<decimal> GetProductOnSalePrice(Product product, 
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CatalogProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var catalogProductId =
                    await GetCatalogProductId(product, context, currentUserService, roleOptions, token);

                if (!catalogProductId.HasValue)
                    return 0m;

                var catalogProduct = await dataLoader.LoadAsync(catalogProductId.Value, token);
                return catalogProduct.OnSalePrice;
            }

            private static async Task<Guid?> GetCatalogProductId(Product product, QueryDbContext context,
                ICurrentUserService currentUserService, IOptionsSnapshot<RoleOptions> roleOptions, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                Guid? catalogProductId = null;
                if (currentUser.Data.IsInRole(roleOptions.Value.Store.Value))
                {
                    var hasAgreement = await context.Agreements
                        .Where(c => 
                            c.StoreId == currentUser.Data.Id 
                            && c.ProducerId == product.ProducerId 
                            && c.Status == AgreementStatus.Accepted)
                        .AnyAsync(token);

                    if (hasAgreement)
                        catalogProductId = await context.Agreements
                            .Where(c => 
                                c.StoreId == currentUser.Data.Id 
                                && c.ProducerId == product.ProducerId 
                                && c.Catalog.Kind == CatalogKind.Stores 
                                && c.Status == AgreementStatus.Accepted)
                            .SelectMany(a => a.Catalog.Products)
                            .Where(c => c.ProductId == product.Id)
                            .Select(c => c.Id)
                            .SingleOrDefaultAsync(token);
                    else
                        catalogProductId = await context.Products
                            .Where(p => p.Id == product.Id)
                            .SelectMany(p => p.CatalogsPrices)
                            .Where(cp =>
                                cp.Catalog.Kind == CatalogKind.Stores 
                                && cp.Catalog.Available 
                                && cp.Catalog.IsDefault)
                            .Select(p => p.Id)
                            .SingleOrDefaultAsync(token);
                }
                else
                {
                    catalogProductId = await context.Set<CatalogProduct>()
                        .Where(cp =>
                            cp.ProductId == product.Id 
                            && cp.Catalog.Kind == CatalogKind.Consumers 
                            && cp.Catalog.Available)
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync(token);
                }

                return catalogProductId;
            }
        }
    }
}
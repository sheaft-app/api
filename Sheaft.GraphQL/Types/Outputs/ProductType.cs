using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.Ratings;
using Sheaft.GraphQL.Returnables;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

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
                .Resolve(c => PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture, PictureSize.MEDIUM))
                .Name("picture");

            descriptor
                .Field("imageLarge")
                .Resolve(c => PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture, PictureSize.LARGE))
                .Type<StringType>();

            descriptor
                .Field("imageMedium")
                .Resolve(c => PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture, PictureSize.MEDIUM))
                .Type<StringType>();

            descriptor
                .Field("imageSmall")
                .Resolve(c => PictureExtensions.GetPictureUrl(c.Parent<Product>().Id, c.Parent<Product>().Picture, PictureSize.SMALL))
                .Type<StringType>();

            descriptor
                .Field("isReturnable")
                .Resolve(c => c.Parent<Product>().ReturnableId.HasValue)
                .Type<BooleanType>();

            descriptor
                .Field("visibleToConsumers")
                .Resolve(c => true)
                .Authorize(Policies.PRODUCER)
                .Type<BooleanType>();

            descriptor
                .Field("visibleToStores")
                .Resolve(c => true)
                .Authorize(Policies.PRODUCER)
                .Type<BooleanType>();

            descriptor
                .Field("vatPricePerUnit")
                .Resolve(c => 0m)
                .Type<DecimalType>();

            descriptor
                .Field("onSalePricePerUnit")
                .Resolve(c => 0m)
                .Type<DecimalType>();

            descriptor
                .Field("wholeSalePricePerUnit")
                .Resolve(c => 0m)
                .Type<DecimalType>();

            descriptor
                .Field("onSalePrice")
                .Resolve(c => 0m)
                .Type<DecimalType>();

            descriptor
                .Field("wholeSalePrice")
                .Resolve(c => 0m)
                .Type<DecimalType>();

            descriptor
                .Field("vatPrice")
                .Resolve(c => 0m)
                .Type<DecimalType>();

            descriptor
                .Field(c => c.CatalogsPrices)
                .Name("catalogPrices")
                .UseDbContext<AppDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetProductCatalogs(default, default, default, default))
                .Type<ListType<CatalogProductType>>();

            descriptor
                .Field("currentUserHasRatedProduct")
                .Type<NonNullType<BooleanType>>()
                .UseDbContext<AppDbContext>()
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
                .UseDbContext<AppDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetTags(default!, default!, default!, default))
                .Type<ListType<TagType>>();

            descriptor
                .Field(c => c.Ratings)
                .Name("ratings")
                .UseDbContext<AppDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetRatings(default!, default!, default, default))
                .Type<ListType<RatingType>>()
                .UsePaging()
                .UseSorting();

            descriptor
                .Field(c => c.Pictures)
                .Name("pictures")
                .UseDbContext<AppDbContext>()
                .ResolveWith<ProductResolvers>(c => c.GetPictures(default!, default!, default, default))
                .Type<ListType<ProductPictureType>>();
        }

        private class ProductResolvers
        {
            public Task<bool> ProductIsRatedByUser(Product product, [ScopedService] AppDbContext context,
                [Service] ICurrentUserService currentUserService, CancellationToken token)
            {
                var currentUserResult = currentUserService.GetCurrentUserInfo();
                if (!currentUserResult.Succeeded)
                    return null;

                return context.Set<Rating>()
                    .AnyAsync(r => r.ProductId == product.Id && r.UserId == currentUserResult.Data.Id, token);
            }

            public async Task<IEnumerable<ProductPicture>> GetPictures(Product product,
                [ScopedService] AppDbContext context,
                ProductPicturesByIdBatchDataLoader picturesDataLoader, CancellationToken token)
            {
                var picturesId = await context.Set<ProductPicture>()
                    .Where(p => p.ProductId == product.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await picturesDataLoader.LoadAsync(picturesId, token);
            }

            public async Task<IEnumerable<CatalogProduct>> GetProductCatalogs(Product product,
                [ScopedService] AppDbContext context, CatalogProductsByIdBatchDataLoader catalogProductsDataLoader,
                CancellationToken token)
            {
                var catalogProductsId = await context.Set<CatalogProduct>()
                    .Where(cp => cp.ProductId == product.Id)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await catalogProductsDataLoader.LoadAsync(catalogProductsId, token);
            }

            public async Task<IEnumerable<Tag>> GetTags(Product product, [ScopedService] AppDbContext context,
                TagsByIdBatchDataLoader tagsDataLoader, CancellationToken token)
            {
                var tagsId = await context.Set<ProductTag>()
                    .Where(p => p.ProductId == product.Id)
                    .Select(p => p.TagId)
                    .ToListAsync(token);

                return await tagsDataLoader.LoadAsync(tagsId, token);
            }

            public async Task<IEnumerable<Rating>> GetRatings(Product product, [ScopedService] AppDbContext context,
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
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.Ratings;
using Sheaft.GraphQL.Returnables;
using Sheaft.GraphQL.Tags;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CatalogProductType : SheaftOutputType<CatalogProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("OrderableProduct");

            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));
            
            descriptor
                .Field(c => c.VatPricePerUnit)
                .Name("vatPricePerUnit");
                
            descriptor
                .Field(c => c.OnSalePricePerUnit)
                .Name("onSalePricePerUnit");
                
            descriptor
                .Field(c => c.WholeSalePricePerUnit)
                .Name("wholeSalePricePerUnit");
                
            descriptor
                .Field(c => c.OnSalePrice)
                .Name("onSalePrice");
                
            descriptor
                .Field(c => c.WholeSalePrice)
                .Name("wholeSalePrice");
                
            descriptor
                .Field(c => c.VatPrice)
                .Name("vatPrice");
            
            descriptor
                .Field(c => c.Product.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Product.Reference)
                .Name("reference")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Product.CreatedOn)
                .Name("createdOn");

            descriptor
                .Field(c => c.Product.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field(c => c.Product.Vat)
                .Name("vat");

            descriptor
                .Field(c => c.Product.Available)
                .Name("available");

            descriptor
                .Field(c => c.Product.RatingsCount)
                .Name("ratingsCount");
            
            descriptor
                .Field(c => c.Product.PicturesCount)
                .Name("picturesCount");
            
            descriptor
                .Field(c => c.Product.CatalogsPricesCount)
                .Name("catalogsCount");

            descriptor
                .Field(c => c.Product.QuantityPerUnit)
                .Name("quantityPerUnit");

            descriptor
                .Field(c => c.Product.Unit)
                .Name("unit");

            descriptor
                .Field(c => c.Product.Conditioning)
                .Name("conditioning");

            descriptor
                .Field(c => c.Product.Description)
                .Name("description");

            descriptor
                .Field(c => c.Product.Weight)
                .Name("weight");

            descriptor
                .Field(c => c.Product.Rating)
                .Name("rating");

            descriptor
                .Field(c => c.Product.Picture)
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<CatalogProduct>().ProductId, c.Parent<CatalogProduct>().Product.Picture,
                        PictureSize.MEDIUM))
                .Name("picture");

            descriptor
                .Field("imageLarge")
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<CatalogProduct>().ProductId, c.Parent<CatalogProduct>().Product.Picture,
                        PictureSize.LARGE))
                .Type<StringType>();

            descriptor
                .Field("imageMedium")
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<CatalogProduct>().ProductId, c.Parent<CatalogProduct>().Product.Picture,
                        PictureSize.MEDIUM))
                .Type<StringType>();

            descriptor
                .Field("imageSmall")
                .Resolve(c =>
                    PictureExtensions.GetPictureUrl(c.Parent<CatalogProduct>().ProductId, c.Parent<CatalogProduct>().Product.Picture,
                        PictureSize.SMALL))
                .Type<StringType>();

            descriptor
                .Field("isReturnable")
                .Resolve(c => c.Parent<CatalogProduct>().Product.ReturnableId.HasValue)
                .Type<BooleanType>();

            descriptor
                .Field("visibleTo")
                .Resolve(c => c.Parent<CatalogProduct>().Product.VisibleTo)
                .Authorize(Policies.PRODUCER);

            descriptor
                .Field("returnable")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogProductResolvers>(c => c.GetReturnable(default, default, default))
                .Type<ReturnableType>();
            
            descriptor
                .Field("producer")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogProductResolvers>(c => c.GetProducer(default, default, default))
                .Type<NonNullType<ProducerType>>();

            descriptor
                .Field("currentUserHasRatedProduct")
                .Type<BooleanType>()
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogProductResolvers>(c => c.ProductIsRatedByUser(default, default, default!, default));

            descriptor
                .Field("tags")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogProductResolvers>(c => c.GetTags(default!, default!, default!, default))
                .Type<ListType<TagType>>();

            descriptor
                .Field("ratings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogProductResolvers>(c => c.GetRatings(default!, default!, default, default))
                .Type<ListType<RatingType>>()
                .UsePaging()
                .UseSorting();

            descriptor
                .Field("pictures")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogProductResolvers>(c => c.GetPictures(default!, default!, default, default))
                .Type<ListType<ProductPictureType>>();
        }

        private class CatalogProductResolvers
        {
            public Task<bool> ProductIsRatedByUser(CatalogProduct product, [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService, CancellationToken token)
            {
                var currentUserResult = currentUserService.GetCurrentUserInfo();
                if (!currentUserResult.Succeeded)
                    return null;

                return context.Set<Rating>()
                    .AnyAsync(r => r.ProductId == product.ProductId && r.UserId == currentUserResult.Data.Id, token);
            }

            public async Task<IEnumerable<ProductPicture>> GetPictures(CatalogProduct product,
                [ScopedService] QueryDbContext context,
                ProductPicturesByIdBatchDataLoader picturesDataLoader, CancellationToken token)
            {
                var picturesId = await context.Set<ProductPicture>()
                    .Where(p => p.ProductId == product.ProductId)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await picturesDataLoader.LoadAsync(picturesId, token);
            }

            public async Task<IEnumerable<Tag>> GetTags(CatalogProduct product, [ScopedService] QueryDbContext context,
                TagsByIdBatchDataLoader tagsDataLoader, CancellationToken token)
            {
                var tagsId = await context.Set<ProductTag>()
                    .Where(p => p.ProductId == product.ProductId && !p.Tag.RemovedOn.HasValue)
                    .Select(p => p.TagId)
                    .ToListAsync(token);

                return await tagsDataLoader.LoadAsync(tagsId, token);
            }

            public async Task<IEnumerable<Rating>> GetRatings(CatalogProduct product, [ScopedService] QueryDbContext context,
                RatingsByIdBatchDataLoader ratingsDataLoader, CancellationToken token)
            {
                var ratingsId = await context.Set<Rating>()
                    .Where(p => p.ProductId == product.ProductId)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await ratingsDataLoader.LoadAsync(ratingsId, token);
            }

            public Task<Producer> GetProducer(CatalogProduct product, ProducersByIdBatchDataLoader producersDataLoader,
                CancellationToken token)
            {
                return producersDataLoader.LoadAsync(product.Product.ProducerId, token);
            }

            public Task<Returnable> GetReturnable(CatalogProduct product, ReturnablesByIdBatchDataLoader returnablesDataLoader,
                CancellationToken token)
            {
                if (!product.Product.ReturnableId.HasValue)
                    return null;

                return returnablesDataLoader.LoadAsync(product.Product.ReturnableId.Value, token);
            }
        }
    }
}
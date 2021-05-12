using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CatalogType : SheaftOutputType<Catalog>
    {
        protected override void Configure(IObjectTypeDescriptor<Catalog> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<CatalogsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

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
                .Field(c => c.Available)
                .Name("available");

            descriptor
                .Field(c => c.IsDefault)
                .Name("isDefault");
            
            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");

            descriptor
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogResolvers>(c => c.GetCatalogProducts(default, default, default, default))
                .Type<ListType<ProductPriceType>>();
            
            descriptor
                .Field(c => c.Producer)
                .Name("producer")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<CatalogResolvers>(c => c.GetProducer(default, default, default))
                .Type<UserType>();
        }

        private class CatalogResolvers
        {
            public async Task<IEnumerable<CatalogProduct>> GetCatalogProducts(Catalog catalog, [ScopedService] QueryDbContext context,
                CatalogProductsByIdBatchDataLoader catalogProductsDataLoader, CancellationToken token)
            {
                var catalogProductsId = await context.Set<CatalogProduct>()
                    .Where(cp => cp.CatalogId == catalog.Id && !cp.Product.RemovedOn.HasValue)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await catalogProductsDataLoader.LoadAsync(catalogProductsId, token);
            }
            
            public Task<User> GetProducer(Catalog catalog, UsersByIdBatchDataLoader producersDataLoader, CancellationToken token)
            {
                return producersDataLoader.LoadAsync(catalog.ProducerId, token);
            }
        }
    }
}
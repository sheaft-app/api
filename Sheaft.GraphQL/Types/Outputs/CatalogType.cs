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
using Sheaft.GraphQL.Enums;
using Sheaft.GraphQL.Products;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CatalogType : ObjectType<Catalog>
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
                .Name("kind")
                .Type<NonNullType<CatalogKindEnumType>>();

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
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<AppDbContext>()
                .ResolveWith<CatalogResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<ProductType>>();
        }

        private class CatalogResolvers
        {
            public async Task<IEnumerable<Product>> GetProducts(Catalog catalog, [ScopedService] AppDbContext context,
                ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<CatalogProduct>()
                    .Where(cp => cp.CatalogId == catalog.Id)
                    .Select(cp => cp.ProductId)
                    .ToListAsync(token);

                return await productsDataLoader.LoadAsync(productsId, token);
            }
        }
    }
}
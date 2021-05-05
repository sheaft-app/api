using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Products
{
    [ExtendObjectType(Name = "Query")]
    public class ProductQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public ProductQueries(
            ICurrentUserService currentUserService,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("product")]
        [GraphQLType(typeof(ProductType))]
        [UseDbContext(typeof(AppDbContext))]
        [UseSingleOrDefault]
        public async Task<IQueryable<Product>> Get([ID] Guid id, [ScopedService] AppDbContext context,
            CancellationToken token)
        {
            SetLogTransaction(id);
            if (CurrentUser.IsInRole(_roleOptions.Store.Value))
            {
                var hasAgreement = await context.Agreements
                    .Where(c => c.StoreId == CurrentUser.Id && c.Catalog.Products.Any(p => p.ProductId == id))
                    .AnyAsync(token);

                if (hasAgreement)
                    return context.Agreements
                        .Where(c => c.StoreId == CurrentUser.Id && c.Catalog.Products.Any(p => p.ProductId == id))
                        .SelectMany(a => a.Catalog.Products)
                        .Select(c => c.Product);

                return context.Products
                    .Where(p => p.Id == id && p.CatalogsPrices.Any(cp =>
                        cp.Catalog.Kind == CatalogKind.Stores && cp.Catalog.Available && cp.Catalog.IsDefault));
            }

            return context.Products
                .Where(c => c.Id == id);
        }

        [GraphQLName("products")]
        [GraphQLType(typeof(ListType<ProductType>))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Product> GetAll([ScopedService] AppDbContext context)
        {
            SetLogTransaction();
            
            return context.Products
                .Where(c => c.ProducerId == CurrentUser.Id);
        }

        [GraphQLName("searchProducts")]
        [UseDbContext(typeof(AppDbContext))]
        [GraphQLType(typeof(ProductsSearchDtoType))]
        public async Task<ProductsSearchDto> SearchAsync(
            [GraphQLType(typeof(SearchProductsInputType))] [GraphQLName("input")]
            SearchProductsDto terms,
            [ScopedService] AppDbContext context,
            CancellationToken token)
        {
            var query = context.Products
                .Where(c => c.Available
                            && c.Producer.CanDirectSell
                            && c.CatalogsPrices.Any(cp =>
                                cp.Catalog.Kind == CatalogKind.Consumers && cp.Catalog.Available));

            if (!string.IsNullOrWhiteSpace(terms.Text))
                query = query.Where(p => p.Name.Contains(terms.Text));

            if (terms.ProducerId.HasValue)
                query = query.Where(p => p.ProducerId == terms.ProducerId.Value);

            if (terms.Tags != null && terms.Tags.Any())
                query = query.Where(p => p.Tags.Any(t => terms.Tags.Contains(t.Tag.Name)));

            Point currentPosition = null;
            if (terms.Longitude.HasValue && terms.Latitude.HasValue)
            {
                currentPosition = LocationProvider.CreatePoint(terms.Latitude.Value, terms.Longitude.Value);
                query = query.Where(p => p.Producer.Address.Location.Distance(currentPosition) < 200000);
            }

            var count = await query.CountAsync(token);

            if (!string.IsNullOrWhiteSpace(terms.Sort))
            {
                if (terms.Sort.Contains("producer_geolocation") && currentPosition != null)
                    query = query.OrderBy(p => p.Producer.Address.Location.Distance(currentPosition));
                else
                    query = query.OrderBy(p => p.Name);
            }
            else
                query = query.OrderBy(p => p.Name);

            query = query.Skip(((terms.Page ?? 1) - 1) * terms.Take ?? 20);
            query = query.Take(terms.Take ?? 20);

            var results = await query.ToListAsync(token);

            return new ProductsSearchDto
            {
                Count = count,
                Products = results
            };
        }
    }
}
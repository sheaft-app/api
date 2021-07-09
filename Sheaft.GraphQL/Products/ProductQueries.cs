using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Jobs;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Options;

namespace Sheaft.GraphQL.Products
{
    [ExtendObjectType(Name = "Query")]
    public class ProductQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;
        private readonly SearchOptions _searchOptions;

        public ProductQueries(
            ICurrentUserService currentUserService,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IOptionsSnapshot<SearchOptions> searchOptions,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
            _searchOptions = searchOptions.Value;
        }

        [GraphQLName("product")]
        [GraphQLType(typeof(ProductType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Product> Get([ID] Guid id, [ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            SetLogTransaction(id);
            return context.Products
                .Where(c => c.Id == id);
        }

        [GraphQLName("products")]
        [GraphQLType(typeof(ListType<ProductType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Product>> GetAll([ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();
            if (CurrentUser.IsInRole(_roleOptions.Store.Value))
            {
                var hasAgreements = await context.Agreements
                    .AnyAsync(c => c.StoreId == CurrentUser.Id && c.Status == AgreementStatus.Accepted, token);

                if (hasAgreements)
                    return context.Agreements
                        .Where(c => c.StoreId == CurrentUser.Id && c.Status == AgreementStatus.Accepted)
                        .SelectMany(a => a.Catalog.Products)
                        .Where(c => !c.Product.RemovedOn.HasValue)
                        .Select(c => c.Product);

                return new List<Product>().AsQueryable();
            }

            return context.Products
                .Where(c => c.ProducerId == CurrentUser.Id);
        }

        [GraphQLName("storeOrderableProducts")]
        [GraphQLType(typeof(ListType<CatalogProductType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<CatalogProduct>> GetStoreOrderableProducts([ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            SetLogTransaction();

            var hasAgreements = await context.Agreements
                .AnyAsync(c => c.StoreId == CurrentUser.Id && c.Status == AgreementStatus.Accepted, token);

            if (hasAgreements)
                return context.Agreements
                    .Where(c => c.StoreId == CurrentUser.Id && c.Status == AgreementStatus.Accepted)
                    .SelectMany(a => a.Catalog.Products)
                    .Where(c => !c.Product.RemovedOn.HasValue)
                    .Select(c => c)
                    .Include(c => c.Product);

            return new List<CatalogProduct>().AsQueryable();
        }

        [GraphQLName("searchProducts")]
        [UseDbContext(typeof(QueryDbContext))]
        [GraphQLType(typeof(ProductsSearchDtoType))]
        public async Task<ProductsSearchDto> SearchAsync(
            [GraphQLType(typeof(SearchProductsInputType))] [GraphQLName("input")]
            SearchProductsDto terms,
            [ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            SetLogTransaction(terms);
            var query = context.ConsumerProducts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(terms.Text))
                query = query.Where(p => p.Name.Contains(terms.Text));

            if (terms.ProducerId.HasValue)
                query = query.Where(p => p.ProducerId == terms.ProducerId.Value);

            if (terms.Tags != null && terms.Tags.Any())
            {
                foreach (var tag in terms.Tags)
                    query = query.Where(p => p.Tags.Contains(tag));
            }

            Point currentPosition = null;
            if (terms.Longitude.HasValue && terms.Latitude.HasValue)
            {
                currentPosition = LocationProvider.CreatePoint(terms.Latitude.Value, terms.Longitude.Value);
                query = query.Where(p => p.Location.Distance(currentPosition) < _searchOptions.ProductsDistance);
            }

            var count = await query.CountAsync(token);
            
            if (!string.IsNullOrWhiteSpace(terms.Sort))
            {
                var sort = terms.Sort.ToLowerInvariant();
                if (sort.Contains("producer_geolocation") && currentPosition != null)
                    query = query.OrderBy(p => p.Location.Distance(currentPosition));
                else if (sort.Contains("price") && sort.Contains("asc"))
                    query = query.OrderBy(p => p.OnSalePricePerUnit);
                else if (sort.Contains("price") && sort.Contains("desc"))
                    query = query.OrderByDescending(p => p.OnSalePricePerUnit);
                else
                    query = query.OrderBy(p => p.Name);
            }
            else
                query = query.OrderBy(p => p.Name);

            query = query.Skip(((terms.Page ?? 1) - 1) * terms.Take ?? 20);
            query = query.Take(terms.Take ?? 20);

            var results = await query
                .Select(p => p.Id)
                .ToListAsync(token);

            var products = await context.Products.Where(p => results.Contains(p.Id)).ToListAsync(token);
            var orderedProducts = results.Select(result => products.SingleOrDefault(p => p.Id == result)).ToList();

            return new ProductsSearchDto
            {
                Count = count,
                Products = orderedProducts
            };
        }

        [GraphQLName("importProducts")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(JobType))]
        public async Task<Job> ImportProducts(IFile file, [Service] ISheaftMediatr mediatr, 
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var content = new MemoryStream())
                {
                    await stream.CopyToAsync(content, token);
                    var input = new QueueImportProductsCommand(CurrentUser)
                    {
                        ProducerId = CurrentUser.Id, FileName = file.Name, FileStream = content.ToArray()
                    };

                    var result = await mediatr.Process(input, token);
                    return await jobsDataLoader.LoadAsync(result.Data, token);
                }
            }
        }
    }
}
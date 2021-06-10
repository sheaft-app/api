using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Stores
{
    [ExtendObjectType(Name = "Query")]
    public class StoreQueries : SheaftQuery
    {
        private readonly HttpClient _httpClient;
        private readonly SireneOptions _sireneOptions;
        private readonly SearchOptions _searchOptions;

        public StoreQueries(
            ICurrentUserService currentUserService,
            IOptionsSnapshot<SireneOptions> sireneOptions,
            IOptionsSnapshot<SearchOptions> searchOptions,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
            _sireneOptions = sireneOptions.Value;
            _searchOptions = searchOptions.Value;

            _httpClient = httpClientFactory.CreateClient("sirene");
            _httpClient.BaseAddress = new Uri(_sireneOptions.Url);
            _httpClient.SetToken(_sireneOptions.Scheme, _sireneOptions.ApiKey);
        }

        [GraphQLName("store")]
        [GraphQLType(typeof(StoreType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Store> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Stores
                .Where(c => c.Id == id);
        }

        [GraphQLName("suggestStores")]
        [UseDbContext(typeof(QueryDbContext))]
        [GraphQLType(typeof(ListType<UserType>))]
        public async Task<IEnumerable<User>> SuggestStoresAsync(
            [GraphQLType(typeof(SearchTermsInputType))] [GraphQLName("input")]
            SearchTermsDto terms,
            [ScopedService] QueryDbContext context, CancellationToken token)
        {
            var query = context.Stores.AsQueryable();
            if (!string.IsNullOrWhiteSpace(terms.Text))
                query = query.Where(c => c.Name.Contains(terms.Text));

            return await query.ToListAsync(token);
        }

        [GraphQLName("searchStores")]
        [UseDbContext(typeof(QueryDbContext))]
        [GraphQLType(typeof(StoresSearchDtoType))]
        public async Task<StoresSearchDto> SearchAsync(
            [GraphQLType(typeof(SearchTermsInputType))] [GraphQLName("input")]
            SearchTermsDto terms,
            [ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            var query = context.Stores.Where(c => c.OpenForNewBusiness);

            if (!string.IsNullOrWhiteSpace(terms.Text))
                query = query.Where(p => p.Name.Contains(terms.Text));

            if (terms.Tags != null && terms.Tags.Any())
                query = query.Where(p => p.Tags.Any(t => terms.Tags.Contains(t.Tag.Name)));

            var producer = await context.Producers.SingleAsync(e => e.Id == CurrentUser.Id, token);
            Point currentPosition = null;
            if (producer.Address?.Latitude != null && producer.Address?.Longitude != null)
            {
                currentPosition =
                    LocationProvider.CreatePoint(producer.Address.Latitude.Value, producer.Address.Longitude.Value);
                query = query.Where(p => p.Address.Location.Distance(currentPosition) < _searchOptions.StoresDistance);
            }

            var count = await query.CountAsync(token);

            if (!string.IsNullOrWhiteSpace(terms.Sort))
            {
                if (terms.Sort.Contains("store_geolocation") && currentPosition != null)
                    query = query.OrderBy(p => p.Address.Location.Distance(currentPosition));
                else
                    query = query.OrderBy(p => p.Name);
            }
            else
                query = query.OrderBy(p => p.Name);

            query = query.Skip(((terms.Page ?? 1) - 1) * terms.Take ?? 20);
            query = query.Take(terms.Take ?? 20);

            var results = await query.ToListAsync(token);
            return new StoresSearchDto
            {
                Count = count,
                Stores = results
            };
        }
    }
}
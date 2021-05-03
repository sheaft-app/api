using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Producers
{
    [ExtendObjectType(Name = "Query")]
    public class ProducerQueries : SheaftQuery
    {
        private readonly HttpClient _httpClient;
        private readonly ISearchIndexClient _producersIndex;
        private readonly SireneOptions _sireneOptions;
        private readonly SearchOptions _searchOptions;

        public ProducerQueries(
            ICurrentUserService currentUserService,
            IOptionsSnapshot<SireneOptions> sireneOptions,
            IOptionsSnapshot<SearchOptions> searchOptions,
            IHttpClientFactory httpClientFactory,
            ISearchServiceClient searchServiceClient,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
            _sireneOptions = sireneOptions.Value;
            _searchOptions = searchOptions.Value;

            _httpClient = httpClientFactory.CreateClient("sirene");
            _httpClient.BaseAddress = new Uri(_sireneOptions.Url);
            _httpClient.SetToken(_sireneOptions.Scheme, _sireneOptions.ApiKey);

            _producersIndex = searchServiceClient.Indexes.GetClient(_searchOptions.Indexes.Producers);
        }

        [GraphQLName("producer")]
        [GraphQLType(typeof(ProducerType))]
        [UseDbContext(typeof(AppDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Producer> Get([ID] Guid id, [ScopedService] AppDbContext context)
        {
            SetLogTransaction(id);
            return context.Producers
                .Where(c => c.Id == id);
        }

        [GraphQLName("suggestProducers")]
        [GraphQLType(typeof(ListType<SuggestProducerDtoType>))]
        public async Task<IEnumerable<SuggestProducerDto>> SuggestProducersAsync(
            [GraphQLName("input")] SearchTermsDto terms, CancellationToken token)
        {
            var sp = new SuggestParameters
            {
                Top = terms.Take,
                SearchFields = new List<string> {"producer_name"},
                Select = new List<string>()
                {
                    "producer_id", "producer_name", "producer_zipcode", "producer_city"
                },
                HighlightPreTag = "<b>",
                HighlightPostTag = "</b>",
                UseFuzzyMatching = true,
                OrderBy = new List<string> {"producer_name desc"},
                Filter = "removed eq 0"
            };

            var results = await _producersIndex.Documents.SuggestAsync(terms.Text, _searchOptions.Suggesters.Producers,
                sp, cancellationToken: token);
            var searchResults = new List<SearchProducer>();
            foreach (var result in results.Results)
            {
                var json = JsonConvert.SerializeObject(result.Document, Formatting.None);
                searchResults.Add(JsonConvert.DeserializeObject<SearchProducer>(json));
            }

            return searchResults.Select(p => new SuggestProducerDto
            {
                Id = p.Producer_id,
                Name = p.Producer_name,
                Address = new SuggestAddressDto
                {
                    City = p.Producer_city,
                    Zipcode = p.Producer_zipcode
                }
            }) ?? new List<SuggestProducerDto>();
        }

        [GraphQLName("searchProducers")]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(ListType<ProducersSearchDtoType>))]
        public async Task<ProducersSearchDto> SearchProducersAsync(
            [GraphQLType(typeof(SearchTermsInputType))] [GraphQLName("input")] SearchTermsDto terms,
            [ScopedService] AppDbContext context, CancellationToken token)
        {
            var sp = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Top = terms.Take,
                Skip = (terms.Page - 1) * terms.Take,
                SearchFields = new List<string> {"partialProducerName"},
                Select = new List<string>()
                {
                    "producer_id", "producer_name", "producer_email", "producer_phone", "producer_picture",
                    "producer_tags", "producer_line1", "producer_line2", "producer_zipcode", "producer_city",
                    "producer_longitude", "producer_latitude", "producer_products_count"
                },
                IncludeTotalResultCount = true,
                HighlightFields = new List<string>() {"producer_name"},
                HighlightPreTag = "<b>",
                HighlightPostTag = "</b>"
            };

            var store = await context.Stores.SingleAsync(e => e.Id == CurrentUser.Id, token);
            if (!string.IsNullOrWhiteSpace(terms.Sort))
            {
                if (terms.Sort.Contains("producer_geolocation"))
                {
                    sp.OrderBy = new List<string>()
                    {
                        $"geo.distance(producer_geolocation, geography'POINT({store.Address.Longitude.Value.ToString(new CultureInfo("en-US"))} {store.Address.Latitude.Value.ToString(new CultureInfo("en-US"))})')"
                    };
                }
                else if (!terms.Sort.Contains("producer_geolocation"))
                {
                    sp.OrderBy = new List<string>() {terms.Sort};
                }
            }

            var filter = "removed eq 0";
            if (terms.Tags != null)
            {
                foreach (var tag in terms.Tags)
                {
                    filter += " and ";
                    filter += $"producer_tags/any(p: p eq '{tag.ToLowerInvariant()}')";
                }
            }

            filter += " and ";
            filter +=
                $"geo.distance(producer_geolocation, geography'POINT({store.Address.Longitude.Value.ToString(new CultureInfo("en-US"))} {store.Address.Latitude.Value.ToString(new CultureInfo("en-US"))})') le {terms.MaxDistance ?? 200}";

            sp.Filter = filter;

            var results = await _producersIndex.Documents.SearchAsync(terms.Text, sp, cancellationToken: token);
            var searchResults = new List<SearchProducer>();
            foreach (var result in results.Results)
            {
                var json = JsonConvert.SerializeObject(result.Document, Formatting.None);
                searchResults.Add(JsonConvert.DeserializeObject<SearchProducer>(json));
            }

            return new ProducersSearchDto
            {
                Count = results.Count ?? 0,
                Producers = searchResults?.Select(p => new ProducerDto
                {
                    Id = p.Producer_id,
                    Name = p.Producer_name,
                    Email = p.Producer_email,
                    Phone = p.Producer_phone,
                    Picture = p.Producer_picture != null ? p.Producer_picture : string.Empty,
                    Tags = p.Producer_tags?.Select(t => new TagDto {Name = t}) ?? new List<TagDto>(),
                    Address = new AddressDto
                    {
                        Line1 = p.Producer_line1,
                        Line2 = p.Producer_line2,
                        City = p.Producer_city,
                        Latitude = p.Producer_latitude,
                        Longitude = p.Producer_longitude,
                        Zipcode = p.Producer_zipcode
                    }
                }) ?? new List<ProducerDto>()
            };
        }

        private class SearchProducer
        {
            public Guid Producer_id { get; set; }
            public string Producer_name { get; set; }
            public string Producer_line1 { get; set; }
            public string Producer_line2 { get; set; }
            public string Producer_zipcode { get; set; }
            public string Producer_city { get; set; }
            public string Producer_email { get; set; }
            public string Producer_phone { get; set; }
            public double Producer_longitude { get; set; }
            public double Producer_latitude { get; set; }
            public string Producer_picture { get; set; }
            public int Producer_products_count { get; set; }
            public IEnumerable<string> Producer_tags { get; set; }
        }
    }
}
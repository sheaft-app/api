using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using IdentityModel.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Store.Queries
{
    public class StoreQueries : IStoreQueries
    {
        private readonly ISearchIndexClient _storesIndex;
        private readonly IAppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly SireneOptions _sireneOptions;
        private readonly SearchOptions _searchOptions;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public StoreQueries(
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<SireneOptions> sireneOptions,
            IOptionsSnapshot<SearchOptions> searchOptions,
            ISearchServiceClient searchServiceClient,
            IAppDbContext context,
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;

            _sireneOptions = sireneOptions.Value;
            _searchOptions = searchOptions.Value;

            _httpClient = httpClientFactory.CreateClient("sirene");
            _httpClient.BaseAddress = new Uri(_sireneOptions.Url);
            _httpClient.SetToken(_sireneOptions.Scheme, _sireneOptions.ApiKey);

            _storesIndex = searchServiceClient.Indexes.GetClient(_searchOptions.Indexes.Stores);

            _context = context;
        }

        public async Task<StoresSearchDto> SearchStoresAsync(Guid producerId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token)
        {
            var sp = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Top = terms.Take,
                Skip = (terms.Page - 1) * terms.Take,
                SearchFields = new List<string> { "partialStoreName" },
                Select = new List<string>()
                    {
                        "store_id", "store_name", "store_email", "store_phone",  "store_picture", "store_tags", "store_line1", "store_line2", "store_zipcode", "store_city", "store_longitude", "store_latitude"
                    },
                IncludeTotalResultCount = true,
                HighlightFields = new List<string>() { "store_name" },
                HighlightPreTag = "<b>",
                HighlightPostTag = "</b>"
            };

            var producer = await _context.GetByIdAsync<Domain.Producer>(producerId, token);
            if (!string.IsNullOrWhiteSpace(terms.Sort))
            {
                if (terms.Sort.Contains("store_geolocation"))
                {
                    sp.OrderBy = new List<string>() { $"geo.distance(store_geolocation, geography'POINT({producer.Address.Longitude.Value.ToString(new CultureInfo("en-US"))} {producer.Address.Latitude.Value.ToString(new CultureInfo("en-US"))})')" };
                }
                else if (!terms.Sort.Contains("store_geolocation"))
                {
                    sp.OrderBy = new List<string>() { terms.Sort };
                }
            }

            var filter = "removed eq 0";
            if (terms.Tags != null)
            {
                foreach (var tag in terms.Tags)
                {
                    filter += " and ";
                    filter += $"store_tags/any(p: p eq '{tag.ToLowerInvariant()}')";
                }
            }

            filter += " and ";
            filter += $"geo.distance(store_geolocation, geography'POINT({producer.Address.Longitude.Value.ToString(new CultureInfo("en-US"))} {producer.Address.Latitude.Value.ToString(new CultureInfo("en-US"))})') le {terms.MaxDistance ?? 200}";

            sp.Filter = filter;

            var results = await _storesIndex.Documents.SearchAsync(terms.Text, sp, cancellationToken: token);
            var searchResults = new List<SearchStore>();
            foreach (var result in results.Results)
            {
                var json = JsonConvert.SerializeObject(result.Document, Formatting.None);
                searchResults.Add(JsonConvert.DeserializeObject<SearchStore>(json));
            }

            return new StoresSearchDto
            {
                Count = results.Count ?? 0,
                Stores = searchResults?.Select(p => new StoreDto
                {
                    Id = p.Store_id,
                    Name = p.Store_name,
                    Email = p.Store_email,
                    Phone = p.Store_phone,
                    Picture = p.Store_picture != null ? p.Store_picture : string.Empty,
                    Tags = p.Store_tags?.Select(t => new TagDto { Name = t }) ?? new List<TagDto>(),
                    Address = new AddressDto
                    {
                        Line1 = p.Store_line1,
                        Line2 = p.Store_line2,
                        City = p.Store_city,
                        Latitude = p.Store_latitude,
                        Longitude = p.Store_longitude,
                        Zipcode = p.Store_zipcode
                    }
                }) ?? new List<StoreDto>()
            };
        }

        public IQueryable<StoreDto> GetStore(Guid id, RequestUser currentUser)
        {
            return _context.Users.OfType<Domain.Store>()
                    .Get(c => c.Id == id)
                    .ProjectTo<StoreDto>(_configurationProvider);
        }

        private class SearchStore
        {
            public Guid Store_id { get; set; }
            public string Store_name { get; set; }
            public string Store_line1 { get; set; }
            public string Store_line2 { get; set; }
            public string Store_zipcode { get; set; }
            public string Store_city { get; set; }
            public string Store_email { get; set; }
            public string Store_phone { get; set; }
            public double Store_longitude { get; set; }
            public double Store_latitude { get; set; }
            public string Store_picture { get; set; }
            public IEnumerable<string> Store_tags { get; set; }
        }
    }
}
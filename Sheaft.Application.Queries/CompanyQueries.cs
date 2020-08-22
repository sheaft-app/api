using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using Sheaft.Infrastructure.Interop;
using Sheaft.Core;
using Sheaft.Interop.Enums;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class CompanyQueries : ICompanyQueries
    {
        private readonly HttpClient _httpClient;
        private readonly ISearchIndexClient _producersIndex;
        private readonly ISearchIndexClient _storesIndex;
        private readonly IAppDbContext _context;
        private readonly SireneOptions _sireneOptions;
        private readonly SearchOptions _searchOptions;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public CompanyQueries(
            IOptionsSnapshot<SireneOptions> sireneOptions,
            IOptionsSnapshot<SearchOptions> searchOptions,
            IHttpClientFactory httpClientFactory,
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

            _producersIndex = searchServiceClient.Indexes.GetClient(_searchOptions.Indexes.Producers);
            _storesIndex = searchServiceClient.Indexes.GetClient(_searchOptions.Indexes.Stores);

            _context = context;
        }

        public async Task<SirenCompanyDto> RetrieveSiretCompanyInfosAsync(string siret, RequestUser currentUser, CancellationToken token)
        {
            try
            {
                var result = await _httpClient.GetAsync(string.Format(_sireneOptions.SearchSiretUrl, siret), token);
                if (!result.IsSuccessStatusCode)
                    return null;

                var content = await result.Content.ReadAsStringAsync();
                var contentObj = JsonConvert.DeserializeObject<CompanySiretResult>(content);
                return contentObj.Etablissement;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<StoresSearchDto> SearchStoresAsync(Guid companyId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token)
        {
            try
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

                var producer = await _context.GetByIdAsync<Company>(companyId, token);
                if (!string.IsNullOrWhiteSpace(terms.Sort))
                {
                    if (terms.Sort.Contains("store_geolocation"))
                    {
                        sp.OrderBy = new List<string>(){  "geo.distance(store_geolocation, geography'POINT(" + producer.Address.Longitude.Value.ToString(new CultureInfo("en-US")) + " " + producer.Address.Latitude.Value.ToString(new CultureInfo("en-US")) +
                              ")')" };
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
                        filter += "store_tags/any(p: p eq '" + tag.ToLowerInvariant() + "')";
                    }
                }

                filter += " and ";
                filter += "geo.distance(store_geolocation, geography'POINT(" + producer.Address.Longitude.Value.ToString(new CultureInfo("en-US")) + " " + producer.Address.Latitude.Value.ToString(new CultureInfo("en-US")) +
                          ")') le " + (terms.MaxDistance ?? 200);

                sp.Filter = filter;

                var results = await _storesIndex.Documents.SearchAsync(terms.Text, sp, cancellationToken: token);
                var searchResults = new List<SearchStore>();
                foreach (var result in results.Results)
                {
                    var json = JsonConvert.SerializeObject(result.Document, Newtonsoft.Json.Formatting.Indented);
                    var myobject = JsonConvert.DeserializeObject<SearchStore>(json);
                    searchResults.Add(myobject);
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
                        Picture = p.Store_picture,
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
                    })
                };
            }
            catch (Exception ex)
            {
                return new StoresSearchDto();
            }
        }

        public async Task<ProducersSearchDto> SearchProducersAsync(Guid companyId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token)
        {
            try
            {
                var sp = new SearchParameters()
                {
                    SearchMode = SearchMode.Any,
                    Top = terms.Take,
                    Skip = (terms.Page - 1) * terms.Take,
                    SearchFields = new List<string> { "partialProducerName" },
                    Select = new List<string>()
                    {
                        "producer_id", "producer_name", "producer_email", "producer_phone",  "producer_picture", "producer_tags", "producer_line1", "producer_line2", "producer_zipcode", "producer_city", "producer_longitude", "producer_latitude", "producer_products_count"
                    },
                    IncludeTotalResultCount = true,
                    HighlightFields = new List<string>() { "producer_name" },
                    HighlightPreTag = "<b>",
                    HighlightPostTag = "</b>"
                };

                var store = await _context.GetByIdAsync<Company>(companyId, token);
                if (!string.IsNullOrWhiteSpace(terms.Sort))
                {
                    if (terms.Sort.Contains("producer_geolocation"))
                    {
                        sp.OrderBy = new List<string>(){  "geo.distance(producer_geolocation, geography'POINT(" + store.Address.Longitude.Value.ToString(new CultureInfo("en-US")) + " " + store.Address.Latitude.Value.ToString(new CultureInfo("en-US")) +
                              ")')" };
                    }
                    else if (!terms.Sort.Contains("producer_geolocation"))
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
                        filter += "producer_tags/any(p: p eq '" + tag.ToLowerInvariant() + "')";
                    }
                }

                filter += " and ";
                filter += "geo.distance(producer_geolocation, geography'POINT(" + store.Address.Longitude.Value.ToString(new CultureInfo("en-US")) + " " + store.Address.Latitude.Value.ToString(new CultureInfo("en-US")) +
                          ")') le " + (terms.MaxDistance ?? 200);

                sp.Filter = filter;

                var results = await _producersIndex.Documents.SearchAsync(terms.Text, sp, cancellationToken: token);
                var searchResults = new List<SearchProducer>();
                foreach (var result in results.Results)
                {
                    var json = JsonConvert.SerializeObject(result.Document, Formatting.Indented);
                    var myobject = JsonConvert.DeserializeObject<SearchProducer>(json);
                    searchResults.Add(myobject);
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
                        Picture = p.Producer_picture,
                        Tags = p.Producer_tags?.Select(t => new TagDto { Name = t }) ?? new List<TagDto>(),
                        Address = new AddressDto
                        {
                            Line1 = p.Producer_line1,
                            Line2 = p.Producer_line2,
                            City = p.Producer_city,
                            Latitude = p.Producer_latitude,
                            Longitude = p.Producer_longitude,
                            Zipcode = p.Producer_zipcode
                        }
                    })
                };
            }
            catch (Exception ex)
            {
                return new ProducersSearchDto();
            }
        }

        public IQueryable<CompanyDto> GetCompany(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Companies
                        .Get(c => c.Id == id && c.Id == currentUser.CompanyId)
                        .ProjectTo<CompanyDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<CompanyDto>().AsQueryable();
            }
        }

        public IQueryable<CompanyDto> GetCompanies(RequestUser currentUser)
        {
            try
            {
                return _context.Companies
                        .Get(c => c.Id == currentUser.CompanyId)
                        .ProjectTo<CompanyDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<CompanyDto>().AsQueryable();
            }
        }

        public IQueryable<ProducerDto> GetProducer(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Companies
                        .Get(c => c.Id == id && c.Kind == ProfileKind.Producer)
                        .ProjectTo<ProducerDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<ProducerDto>().AsQueryable();
            }
        }

        public IQueryable<StoreDto> GetStore(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Companies
                        .Get(c => c.Id == id && c.Kind == ProfileKind.Store)
                        .ProjectTo<StoreDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<StoreDto>().AsQueryable();
            }
        }

        private static IQueryable<CompanyDto> GetAsDto(IQueryable<Company> query)
        {
            return query
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Address = new AddressDto
                    {
                        City = c.Address.City,
                        Latitude = c.Address.Latitude,
                        Line1 = c.Address.Line1,
                        Line2 = c.Address.Line2,
                        Longitude = c.Address.Longitude,
                        Zipcode = c.Address.Zipcode
                    },
                    UpdatedOn = c.UpdatedOn,
                    RemovedOn = c.RemovedOn,
                    AppearInBusinessSearchResults = c.AppearInBusinessSearchResults,
                    CreatedOn = c.CreatedOn,
                    Description = c.Description,
                    Email = c.Email,
                    Kind = c.Kind,
                    Name = c.Name,
                    OpeningHours = c.OpeningHours.Select(oh => new TimeSlotDto
                    {
                        Day = oh.Day,
                        From = oh.From,
                        To = oh.To
                    }),
                    Phone = c.Phone,
                    Picture = c.Picture,
                    Tags = c.Tags.Select(st => new TagDto
                    {
                        Id = st.Tag.Id,
                        Name = st.Tag.Name,
                        Description = st.Tag.Description,
                        Image = st.Tag.Image,
                        Kind = st.Tag.Kind,
                        UpdatedOn = st.Tag.UpdatedOn,
                        CreatedOn = st.Tag.CreatedOn
                    }),
                    Reason = c.Reason,
                    Siret = c.Siret,
                    VatIdentifier = c.VatIdentifier
                });
        }

        private static IQueryable<StoreDto> GetAsStoreDto(IQueryable<Company> query)
        {
            return query
                .Select(c => new StoreDto
                {
                    Id = c.Id,
                    Address = new AddressDto
                    {
                        City = c.Address.City,
                        Latitude = c.Address.Latitude,
                        Line1 = c.Address.Line1,
                        Line2 = c.Address.Line2,
                        Longitude = c.Address.Longitude,
                        Zipcode = c.Address.Zipcode
                    },
                    Description = c.Description,
                    Email = c.Email,
                    Name = c.Name,
                    OpeningHours = c.OpeningHours.Select(oh => new TimeSlotDto
                    {
                        Day = oh.Day,
                        From = oh.From,
                        To = oh.To
                    }),
                    Phone = c.Phone,
                    Picture = c.Picture,
                    Tags = c.Tags.Select(st => new TagDto
                    {
                        Id = st.Tag.Id,
                        Name = st.Tag.Name,
                        Description = st.Tag.Description,
                        Image = st.Tag.Image,
                        Kind = st.Tag.Kind,
                        UpdatedOn = st.Tag.UpdatedOn,
                        CreatedOn = st.Tag.CreatedOn
                    })
                });
        }

        private static IQueryable<ProducerDto> GetAsProducerDto(IQueryable<Company> query)
        {
            return query
                .Select(c => new ProducerDto
                {
                    Id = c.Id,
                    Address = new AddressDto
                    {
                        City = c.Address.City,
                        Latitude = c.Address.Latitude,
                        Line1 = c.Address.Line1,
                        Line2 = c.Address.Line2,
                        Longitude = c.Address.Longitude,
                        Zipcode = c.Address.Zipcode
                    },
                    Description = c.Description,
                    Email = c.Email,
                    Name = c.Name,
                    Phone = c.Phone,
                    Picture = c.Picture,
                    Tags = c.Tags.Select(st => new TagDto
                    {
                        Id = st.Tag.Id,
                        Name = st.Tag.Name,
                        Description = st.Tag.Description,
                        Image = st.Tag.Image,
                        Kind = st.Tag.Kind,
                        UpdatedOn = st.Tag.UpdatedOn,
                        CreatedOn = st.Tag.CreatedOn
                    })
                });
        }

        private class CompanySiretResult
        {
            public SirenCompanyDto Etablissement { get; set; }
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
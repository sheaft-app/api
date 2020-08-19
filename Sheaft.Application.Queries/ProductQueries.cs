using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Models.Inputs;
using Sheaft.Models.Dto;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class ProductQueries : IProductQueries
    {
        private readonly ISearchIndexClient _indexClient;
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public ProductQueries(IAppDbContext context, IOptionsSnapshot<SearchOptions> searchOptions, ISearchServiceClient searchServiceClient, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
            _indexClient = searchServiceClient.Indexes.GetClient(searchOptions.Value.Indexes.Products);
        }

        public async Task<ProductsSearchDto> SearchAsync(SearchTermsInput terms, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                var sp = new SearchParameters()
                {
                    SearchMode = SearchMode.Any,
                    Top = terms.Take,
                    Skip = (terms.Page - 1) * terms.Take,
                    SearchFields = new List<string> { "partialProductName" },
                    Select = new List<string>()
                    {
                        "product_id", "product_name", "product_onSalePricePerUnit", "product_onSalePrice", "product_rating", "product_ratings_count", "product_image", "product_tags", "producer_id", "producer_name", "producer_email", "producer_phone", "producer_zipcode", "producer_city", "producer_longitude", "producer_latitude", "packaged", "product_unit", "product_quantityPerUnit"
                    },
                    IncludeTotalResultCount = true,
                    HighlightFields = new List<string>() { "product_name", "producer_name" },
                    HighlightPreTag = "<b>",
                    HighlightPostTag = "</b>"
                };

                if (!string.IsNullOrWhiteSpace(terms.Sort))
                {
                    if (terms.Sort.Contains("producer_geolocation") && terms.Longitude.HasValue && terms.Latitude.HasValue)
                    {
                        sp.OrderBy = new List<string>(){  "geo.distance(producer_geolocation, geography'POINT(" + terms.Longitude.Value.ToString(new CultureInfo("en-US")) + " " + terms.Latitude.Value.ToString(new CultureInfo("en-US")) +
                              ")')" };
                    }
                    else if(!terms.Sort.Contains("producer_geolocation"))
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
                        filter += "product_tags/any(p: p eq '" + tag.ToLowerInvariant() + "')";
                    }
                }

                if (terms.Longitude.HasValue && terms.Latitude.HasValue)
                {
                    filter += " and ";
                    filter += "geo.distance(producer_geolocation, geography'POINT(" + terms.Longitude.Value.ToString(new CultureInfo("en-US")) + " " + terms.Latitude.Value.ToString(new CultureInfo("en-US")) +
                              ")') le " + (terms.MaxDistance ?? 200);
                }

                sp.Filter = filter;

                var results = await _indexClient.Documents.SearchAsync(terms.Text, sp, cancellationToken: token);
                var searchResults = new List<SearchProduct>();
                foreach (var result in results.Results)
                {
                    var json = JsonConvert.SerializeObject(result.Document, Formatting.Indented);
                    var myobject = JsonConvert.DeserializeObject<SearchProduct>(json);
                    searchResults.Add(myobject);
                }

                return new ProductsSearchDto {
                    Count = results.Count ?? 0,
                    Products = searchResults?.Select(p => new ProductDto
                    {
                        Id = p.Product_id,
                        Name = p.Product_name,
                        ImageMedium = p.Product_image,
                        Picture = p.Product_image,
                        OnSalePrice = p.Product_onSalePrice ?? 0,
                        OnSalePricePerUnit = p.Product_onSalePricePerUnit ?? 0,
                        QuantityPerUnit = p.Product_quantityPerUnit ?? 0,
                        Rating = p.Product_rating,
                        Packaged = p.Packaged.HasValue ? p.Packaged.Value : false,
                        RatingsCount = p.Product_ratings_count,
                        Tags = p.Product_tags?.Select(t => new TagDto { Name = t }) ?? new List<TagDto>(),
                        Unit = (UnitKind)Enum.Parse(typeof(UnitKind), p.Product_unit.ToLowerInvariant()),
                        Producer = new CompanyProfileDto
                        {
                            Id = p.Producer_id,
                            Name = p.Producer_name,
                            Email = p.Producer_email,
                            Phone = p.Producer_phone,
                            Address = new AddressDto
                            {
                                City = p.Producer_city,
                                Latitude = p.Producer_latitude,
                                Longitude = p.Producer_longitude,
                                Zipcode = p.Producer_zipcode
                            }
                        }
                    })} ?? new ProductsSearchDto();
            }
            catch (Exception ex)
            {
                return new ProductsSearchDto();
            }
        }

        public IQueryable<ProductDto> GetStoreProducts(Guid storeId, IRequestUser currentUser)
        {
            try
            {
                var producerIds = _context.Agreements
                        .Get(c => c.Store.Id == storeId && c.Status == AgreementStatusKind.Accepted)
                        .Select(a => a.Delivery.Producer.Id);

                return _context.Products
                        .Get(p => producerIds.Contains(p.Producer.Id))
                        .ProjectTo<ProductDto>(_configurationProvider);
            }
            catch(Exception e)
            {
                return new List<ProductDto>().AsQueryable();
            }
        }

        public IQueryable<ProductDto> GetProducerProducts(Guid producerId, IRequestUser currentUser)
        {
            try
            {
                return _context.Products
                        .Get(p => p.Producer.Id == producerId)
                        .ProjectTo<ProductDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<ProductDto>().AsQueryable();
            }
        }

        public IQueryable<ProductDto> GetProduct(Guid id, IRequestUser currentUser)
        {
            try
            {
                return _context.Products
                        .Get(c => c.Id == id)
                        .ProjectTo<ProductDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<ProductDto>().AsQueryable();
            }
        }

        public IQueryable<ProductDto> GetProducts(IRequestUser currentUser)
        {
            try
            {
                return _context.Products
                        .Get(c => c.Producer.Id == currentUser.CompanyId)
                        .ProjectTo<ProductDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<ProductDto>().AsQueryable();
            }
        }

        public async Task<bool> ProductIsRatedByUserAsync(Guid id, Guid userId, IRequestUser user, CancellationToken token)
        {
            try
            {
                return await _context.AnyAsync<Product>(p => p.Id == id && p.Ratings.Any(r => r.User.Id == userId), token);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static IQueryable<ProductDto> GetAsDto(IQueryable<Product> query)
        {
            return query
                .Select(c => new ProductDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Available = c.Available,
                    CreatedOn = c.CreatedOn,
                    Description = c.Description,
                    OnSalePrice = c.OnSalePrice,
                    OnSalePricePerUnit = c.OnSalePricePerUnit,
                    Packaged = c.Packaging != null,
                    Packaging = new PackagingDto
                    {
                        Id = c.Packaging.Id,
                        Name = c.Packaging.Name,
                        CreatedOn = c.Packaging.CreatedOn,
                        Description = c.Packaging.Description,
                        OnSalePrice = c.Packaging.OnSalePrice,
                        Vat = c.Packaging.Vat,
                        VatPrice = c.Packaging.VatPrice,
                        WholeSalePrice = c.Packaging.WholeSalePrice
                    },
                    Producer = new CompanyProfileDto
                    {
                        Id = c.Producer.Id,
                        Address = new AddressDto
                        {
                            City = c.Producer.Address.City,
                            Latitude = c.Producer.Address.Latitude,
                            Line1 = c.Producer.Address.Line1,
                            Line2 = c.Producer.Address.Line2,
                            Longitude = c.Producer.Address.Longitude,
                            Zipcode = c.Producer.Address.Zipcode
                        },
                        Email = c.Producer.Email,
                        Name = c.Producer.Name,
                        Phone = c.Producer.Phone,
                        Picture = c.Producer.Picture,
                    },
                    QuantityPerUnit = c.QuantityPerUnit,
                    Rating = c.Rating,
                    Ratings = c.Ratings.Select(c => new RatingDto
                    {
                        CreatedOn = c.CreatedOn,
                        Id = c.Id,
                        UpdatedOn = c.UpdatedOn,
                        User = new UserProfileDto
                        {
                            Id = c.User.Id,
                            Email = c.User.Email,
                            Phone = c.User.Phone,
                            Kind = c.User.Company == null ? ProfileKind.Consumer : (ProfileKind)c.User.Company.Kind,
                            Name = c.User.FirstName + " " + c.User.LastName,
                            ShortName = c.User.FirstName + " " + c.User.LastName.Substring(0, 1) + ".",
                            Picture = c.User.Picture,
                            Initials = c.User.FirstName.Substring(0, 1) + c.User.LastName.Substring(0, 1)
                        },
                        Comment = c.Comment,
                        Value = c.Value
                    }),
                    RatingsCount = c.RatingsCount,
                    Reference = c.Reference,
                    RemovedOn = c.RemovedOn,
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
                    Unit = c.Unit,
                    UpdatedOn = c.UpdatedOn,
                    Vat = c.Vat,
                    VatPrice = c.VatPrice,
                    VatPricePerUnit = c.VatPricePerUnit,
                    Weight = c.Weight,
                    WholeSalePrice = c.WholeSalePrice,
                    WholeSalePricePerUnit = c.WholeSalePricePerUnit
                });
        }

        private class SearchProduct
        {
            public Guid Product_id { get; set; }
            public string Product_name { get; set; }
            public decimal? Product_onSalePrice { get; set; }
            public decimal? Product_onSalePricePerUnit { get; set; }
            public decimal? Product_rating { get; set; }
            public decimal? Product_quantityPerUnit { get; set; }
            public string Product_unit { get; set; }
            public int Product_ratings_count { get; set; }
            public string Product_image { get; set; }
            public IEnumerable<string> Product_tags { get; set; }
            public Guid Producer_id { get; set; }
            public string Producer_name { get; set; }
            public string Producer_zipcode { get; set; }
            public string Producer_city { get; set; }
            public string Producer_email { get; set; }
            public string Producer_phone { get; set; }
            public double Producer_longitude { get; set; }
            public double Producer_latitude { get; set; }
            public bool? Packaged { get; set; }
    }
    }
}
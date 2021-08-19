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
using Microsoft.Azure.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
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
        private readonly SireneOptions _sireneOptions;
        private readonly SearchOptions _searchOptions;

        public ProducerQueries(
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

        [GraphQLName("producer")]
        [GraphQLType(typeof(ProducerType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Producer> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Producers
                .Where(c => c.Id == id);
        }

        [GraphQLName("suggestProducers")]
        [UseDbContext(typeof(QueryDbContext))]
        [GraphQLType(typeof(ListType<ProducerType>))]
        public async Task<IEnumerable<Producer>> SuggestProducersAsync(
            [GraphQLType(typeof(SearchTermsInputType))] [GraphQLName("input")]
            SearchTermsDto terms,
            [ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction(terms);
            
            var query = context.Producers.Where(c => c.ProductsCount > 0);
            if (!string.IsNullOrWhiteSpace(terms.Text))
                query = query.Where(c => c.Name.Contains(terms.Text));

            return await query.ToListAsync(token);
        }

        [GraphQLName("searchProducers")]
        [UseDbContext(typeof(QueryDbContext))]
        [GraphQLType(typeof(ProducersSearchDtoType))]
        public async Task<ProducersSearchDto> SearchAsync(
            [GraphQLType(typeof(SearchTermsInputType))] [GraphQLName("input")]
            SearchTermsDto terms,
            [ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            SetLogTransaction(terms);
            
            var query = context.Catalogs
                .Where(c => c.Kind == CatalogKind.Stores && c.Available && c.Producer.OpenForNewBusiness);

            if (!string.IsNullOrWhiteSpace(terms.Text))
                query = query.Where(p => p.Producer.Name.Contains(terms.Text));

            if (terms.Tags != null && terms.Tags.Any())
                query = query.Where(p => p.Producer.Tags.Any(t => terms.Tags.Contains(t.Tag.Name)));

            var store = await context.Stores.SingleAsync(e => e.Id == CurrentUser.Id, token);
            Point currentPosition = null;
            if (store.Address?.Latitude != null && store.Address?.Longitude != null)
            {
                currentPosition =
                    LocationProvider.CreatePoint(store.Address.Latitude.Value, store.Address.Longitude.Value);
                query = query.Where(p => p.Producer.Address.Location.Distance(currentPosition) < _searchOptions.ProducersDistance);
            }

            var producersId = await query.Select(c => c.Producer.Id).Distinct().ToListAsync(token);

            var finalQuery = context.Producers.Where(p => producersId.Contains(p.Id));

            if (!string.IsNullOrWhiteSpace(terms.Sort))
            {
                if (terms.Sort.Contains("producer_geolocation") && currentPosition != null)
                    finalQuery = finalQuery.OrderBy(p => p.Address.Location.Distance(currentPosition));
                else
                    finalQuery = finalQuery.OrderBy(p => p.Name);
            }
            else
                finalQuery = finalQuery.OrderBy(p => p.Name);

            finalQuery = finalQuery.Skip(((terms.Page ?? 1) - 1) * terms.Take ?? 20);
            finalQuery = finalQuery.Take(terms.Take ?? 20);

            var results = await finalQuery.ToListAsync(token);
            return new ProducersSearchDto
            {
                Count = producersId.Count,
                Producers = results
            };
        }
        
        [GraphQLName("producersDeliveries")]
        [GraphQLType(typeof(ListType<ProducerDeliveriesDtoType>))]
        [UseDbContext(typeof(QueryDbContext))]
        public async Task<IEnumerable<ProducerDeliveriesDto>> GetNextDeliveries([ID(nameof(Producer))]IEnumerable<Guid> ids, IEnumerable<DeliveryKind> kinds, [ScopedService] QueryDbContext context, CancellationToken token, DateTimeOffset? currentDate = null)
        {
            SetLogTransaction(new {Ids = ids, Kinds = kinds});

            var deliveriesModes = await context.DeliveryModes
                    .Where(d =>
                        d.Available
                        && ids.Contains(d.ProducerId)
                        && kinds.Contains(d.Kind))
                    .Include(d => d.DeliveryHours)
                    .Include(d => d.Producer)
                    .ToListAsync(token);

            return deliveriesModes
                .GroupBy(dm => dm.ProducerId)
                .Select(p =>
                {
                    var deliveryMode = p.First();
                    return new ProducerDeliveriesDto
                    {
                        Id = deliveryMode.Producer.Id,
                        Name = deliveryMode.Producer.Name,
                        Deliveries = p.Select(dm => new DeliveryDto
                        {
                            Id = dm.Id,
                            Name = dm.Name,
                            Available = dm.Available,
                            Kind = dm.Kind,
                            DeliveryFeesWholeSalePrice = dm.DeliveryFeesWholeSalePrice,
                            DeliveryFeesVatPrice = dm.DeliveryFeesVatPrice,
                            DeliveryFeesOnSalePrice = dm.DeliveryFeesOnSalePrice,
                            DeliveryFeesMinPurchaseOrdersAmount = dm.DeliveryFeesMinPurchaseOrdersAmount,
                            ApplyDeliveryFeesWhen = dm.ApplyDeliveryFeesWhen,
                            AcceptPurchaseOrdersWithAmountGreaterThan = dm.AcceptPurchaseOrdersWithAmountGreaterThan,
                            Address = dm.Address != null ? new AddressDto
                            {
                                Line1 = dm.Address.Line1,
                                Line2 = dm.Address.Line2,
                                Zipcode = dm.Address.Zipcode,
                                City = dm.Address.City,
                                Country = dm.Address.Country,
                                Latitude = dm.Address.Latitude,
                                Longitude = dm.Address.Longitude
                            } : null,
                            DeliveryHours = dm.DeliveryHours?.Select(dh => new DeliveryHourDto
                            {
                                Day = dh.Day,
                                From = dh.From,
                                To = dh.To,
                                ExpectedDeliveryDate = default
                            })
                        })
                    };
                });
        }
    }
}
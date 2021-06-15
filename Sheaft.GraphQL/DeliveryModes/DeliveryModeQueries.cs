using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.DeliveryModes
{
    [ExtendObjectType(Name = "Query")]
    public class DeliveryModeQueries : SheaftQuery
    {
        private readonly ITableService _tableService;
        private readonly RoleOptions _roleOptions;

        public DeliveryModeQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            ITableService tableService,
            IOptionsSnapshot<RoleOptions> roleOptions)
            :base(currentUserService, httpContextAccessor)
        {
            _tableService = tableService;
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("delivery")]
        [GraphQLType(typeof(DeliveryModeType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<DeliveryMode> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            
            return context.DeliveryModes
                .Where(c => c.ProducerId == CurrentUser.Id && c.Id == id);
        }
        
        [GraphQLName("deliveries")]
        [GraphQLType(typeof(ListType<DeliveryModeType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<DeliveryMode> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            
            return context.DeliveryModes
                .Where(c => c.ProducerId == CurrentUser.Id);
        }

        [GraphQLName("deliveryClosing")]
        [GraphQLType(typeof(DeliveryClosingType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<DeliveryClosing> GetClosing([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Set<DeliveryClosing>()
                .Where(d => d.Id == id);
        }
        
        [GraphQLName("deliveryClosings")]
        [GraphQLType(typeof(ListType<DeliveryClosingType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<DeliveryClosing> GetClosings([ID] Guid deliveryId, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Set<DeliveryClosing>()
                .Where(d => d.DeliveryModeId == deliveryId && d.ClosedTo > DateTimeOffset.UtcNow);
        }
        
        [GraphQLName("nextProducersDeliveries")]
        [GraphQLType(typeof(ListType<ProducerDeliveriesDtoType>))]
        [UseDbContext(typeof(QueryDbContext))]
        public async Task<IEnumerable<ProducerDeliveriesDto>> GetNextDeliveries([ID(nameof(Producer))]IEnumerable<Guid> ids, IEnumerable<DeliveryKind> kinds, [ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();

            var deliveriesModes = new List<DeliveryMode>();
            if (CurrentUser.IsInRole(_roleOptions.Store.Value))
            {
                deliveriesModes = await context.Agreements
                    .Where(d =>
                        d.DeliveryMode.Available
                        && ids.Contains(d.ProducerId)
                        && d.StoreId == CurrentUser.Id
                        && d.Status == AgreementStatus.Accepted
                        && kinds.Contains(d.DeliveryMode.Kind))
                    .Select(d => d.DeliveryMode)
                    .ToListAsync(token);
            }
            
            if(!deliveriesModes.Any())
            {
                deliveriesModes = await context.DeliveryModes
                    .Where(d =>
                        d.Available
                        && ids.Contains(d.ProducerId)
                        && kinds.Contains(d.Kind))
                    .ToListAsync(token);
            }

            return await GetProducersDeliveriesAsync(context, ids, DateTimeOffset.UtcNow, deliveriesModes, token);
        }

        private async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(QueryDbContext context, IEnumerable<Guid> producerIds, DateTimeOffset currentDate,
            IEnumerable<Domain.DeliveryMode> deliveriesMode, CancellationToken token)
        {
            var producers = new List<ProducerDeliveriesDto>();
            var deliveriesProducerIds = deliveriesMode.Select(c => c.ProducerId).Distinct();
            var producerDistinctIds = producerIds.Distinct();
            if (deliveriesProducerIds.Count() != producerDistinctIds.Count())
            {
                var notFoundProducerIds = deliveriesProducerIds.Except(producerDistinctIds);
                var notFoundProducers = await context.Producers
                    .Where(c => notFoundProducerIds.Contains(c.Id))
                    .ToListAsync(token);

                producers.AddRange(notFoundProducers.Select(c => new ProducerDeliveriesDto
                    {Id = c.Id, Name = c.Name, Deliveries = null}));
            }

            foreach (var deliveriesGroup in deliveriesMode.GroupBy(c => c.ProducerId))
            {
                var deliveries = new List<DeliveryDto>();
                var producer = new ProducerDeliveriesDto
                {
                    Id = deliveriesGroup.Key,
                    Name = deliveriesGroup.First().Producer.Name
                };

                foreach (var deliveryMode in deliveriesGroup)
                    deliveries.Add(GetDeliveriesForHours(currentDate, deliveryMode, deliveryMode.DeliveryHours));

                producer.Deliveries = deliveries;
                producers.Add(producer);
            }
            
            if (deliveriesMode.All(dm => !dm.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return producers;

            return await GetNotCapedProducersDeliveriesAsync(context, producers, token);
        }

        private async Task<IEnumerable<ProducerDeliveriesDto>> GetNotCapedProducersDeliveriesAsync(QueryDbContext context,
            IEnumerable<ProducerDeliveriesDto> producersDeliveries, CancellationToken token)
        {
            var deliveryModeIds = producersDeliveries.SelectMany(c => c.Deliveries.Select(d => d.Id));
            var deliveriesMode = await context.DeliveryModes
                    .Where(d => deliveryModeIds.Contains(d.Id))
                    .ToListAsync(token);

            var producerDeliveriesHoursToCheck = new List<Tuple<Guid, Guid, DeliveryHourDto>>();
            foreach (var producer in producersDeliveries)
            {
                var deliveryModesToCheckMaxOrders = deliveriesMode.Where(dm =>
                    producer.Deliveries.Any(d => dm.MaxPurchaseOrdersPerTimeSlot.HasValue && dm.Id == d.Id));
                foreach (var deliveryMode in deliveryModesToCheckMaxOrders)
                {
                    var delivery = producer.Deliveries.FirstOrDefault(d => d.Id == deliveryMode.Id);
                    if (delivery == null)
                        continue;

                    producerDeliveriesHoursToCheck.AddRange(delivery.DeliveryHours.Select(dh =>
                        new Tuple<Guid, Guid, DeliveryHourDto>(producer.Id, delivery.Id, dh)));
                }
            }

            var results = await _tableService.GetCapingDeliveriesInfosAsync(producerDeliveriesHoursToCheck, token);
            if (!results.Succeeded)
                throw results.Exception;

            var filteredProducers = new List<ProducerDeliveriesDto>();
            foreach (var producer in producersDeliveries)
            {
                var deliveries = new List<DeliveryDto>();
                foreach (var delivery in producer.Deliveries)
                {
                    var deliveryMode = deliveriesMode.FirstOrDefault(dm => dm.Id == delivery.Id);
                    if (!deliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                    {
                        deliveries.Add(delivery);
                        continue;
                    }

                    var deliveryHours = new List<DeliveryHourDto>();
                    foreach (var deliveryHour in delivery.DeliveryHours)
                    {
                        var capingDelivery = results.Data
                            .FirstOrDefault(cr => cr.ProducerId == producer.Id
                                                  && cr.DeliveryId == delivery.Id
                                                  && cr.ExpectedDate.Year == deliveryHour.ExpectedDeliveryDate.Year
                                                  && cr.ExpectedDate.Month == deliveryHour.ExpectedDeliveryDate.Month
                                                  && cr.ExpectedDate.Day == deliveryHour.ExpectedDeliveryDate.Day
                                                  && cr.From == deliveryHour.From
                                                  && cr.To == deliveryHour.To);

                        if (capingDelivery == null || capingDelivery.Count < deliveryMode.MaxPurchaseOrdersPerTimeSlot)
                            deliveryHours.Add(deliveryHour);
                    }

                    if (deliveryHours.Count > 0)
                    {
                        delivery.DeliveryHours = deliveryHours;
                        deliveries.Add(delivery);
                    }
                }

                producer.Deliveries = deliveries;
                filteredProducers.Add(producer);
            }

            return filteredProducers;
        }

        private DeliveryDto GetDeliveriesForHours(DateTimeOffset currentDate, DeliveryMode deliveryMode,
            ICollection<DeliveryHours> openingHours)
        {
            return new DeliveryDto
            {
                Id = deliveryMode.Id,
                Kind = deliveryMode.Kind,
                Available = deliveryMode.Available,
                Address = deliveryMode.Address != null
                    ? new AddressDto
                    {
                        City = deliveryMode.Address.City,
                        Latitude = deliveryMode.Address.Latitude,
                        Line1 = deliveryMode.Address.Line1,
                        Line2 = deliveryMode.Address.Line2,
                        Longitude = deliveryMode.Address.Longitude,
                        Zipcode = deliveryMode.Address.Zipcode
                    }
                    : null,
                Name = deliveryMode.Name,
                DeliveryHours = GetAvailableDeliveryHours(openingHours, currentDate,
                    deliveryMode.LockOrderHoursBeforeDelivery, deliveryMode.Producer.Closings, deliveryMode.Closings),
                Closings = deliveryMode.Closings?.Select(c => new ClosingDto
                {
                    Id = c.Id,
                    From = new DateTimeOffset(new DateTime(c.ClosedFrom.Year, c.ClosedFrom.Month, c.ClosedFrom.Day, 0,
                        0, 0, DateTimeKind.Utc)),
                    Reason = c.Reason,
                    To = new DateTimeOffset(new DateTime(c.ClosedTo.Year, c.ClosedTo.Month, c.ClosedTo.Day, 0, 0, 0,
                        DateTimeKind.Utc))
                }) ?? new List<ClosingDto>()
            };
        }

        private IEnumerable<DeliveryHourDto> GetAvailableDeliveryHours(IEnumerable<TimeSlotHour> openingHours,
            DateTimeOffset currentDate, int? lockOrderHoursBeforeDelivery,
            ICollection<BusinessClosing> producerClosings,
            ICollection<DeliveryClosing> deliveryClosings)
        {
            var list = new List<DeliveryHourDto>();
            foreach (var openingHour in openingHours.OrderBy(oh => oh.Day))
            {
                var results = new List<DeliveryHourDto>();
                var increment = 0;
                while (results.Count < 2 && increment < 365)
                {
                    var diff = (int) openingHour.Day + increment - (int) currentDate.DayOfWeek;
                    var result = GetDeliveryHourIfMatch(openingHour, currentDate, lockOrderHoursBeforeDelivery, diff,
                        producerClosings, deliveryClosings);
                    if (result != null)
                        results.Add(result);

                    increment += 7;
                }

                if (results.Any())
                    list.AddRange(results);
            }

            return list.OrderBy(l => l.ExpectedDeliveryDate);
        }

        private DeliveryHourDto GetDeliveryHourIfMatch(TimeSlotHour openingHour,
            DateTimeOffset currentDate, int? lockOrderHoursBeforeDelivery, int diff,
            ICollection<BusinessClosing> producerClosings,
            ICollection<DeliveryClosing> deliveryClosings)
        {
            var targetDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.From.Hours,
                openingHour.From.Minutes, openingHour.From.Seconds).AddDays(diff);
            if (currentDate.AddHours(lockOrderHoursBeforeDelivery ?? 0) >= targetDate)
                return null;

            if (deliveryClosings != null &&
                deliveryClosings.Any(c => targetDate >= c.ClosedFrom && targetDate <= c.ClosedTo))
                return null;

            if (producerClosings != null &&
                producerClosings.Any(c => targetDate >= c.ClosedFrom && targetDate <= c.ClosedTo))
                return null;

            return new DeliveryHourDto
            {
                Day = openingHour.Day,
                ExpectedDeliveryDate = targetDate,
                From = openingHour.From,
                To = openingHour.To
            };
        }
    }
}
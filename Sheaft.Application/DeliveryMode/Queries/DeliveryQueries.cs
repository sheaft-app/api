using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.DeliveryMode.Queries
{
    public class DeliveryQueries : IDeliveryQueries
    {
        private readonly IAppDbContext _context;
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly IConfigurationProvider _configurationProvider;

        public DeliveryQueries(
            IAppDbContext context,
            ICapingDeliveriesService capingDeliveriesService,
            IConfigurationProvider configurationProvider)
        {
            _context = context;
            _capingDeliveriesService = capingDeliveriesService;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<DeliveryModeDto> GetDelivery(Guid id, RequestUser currentUser)
        {
            return _context.DeliveryModes
                    .Get(c => c.Id == id && c.Producer.Id == currentUser.Id)
                    .ProjectTo<DeliveryModeDto>(_configurationProvider);
        }

        public IQueryable<DeliveryModeDto> GetDeliveries(RequestUser currentUser)
        {
            return _context.DeliveryModes
                    .Get(c => c.Producer.Id == currentUser.Id)
                    .ProjectTo<DeliveryModeDto>(_configurationProvider);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token)
        {
            var producers = new List<ProducerDeliveriesDto>();
            var deliveriesMode = await _context.FindAsync<Domain.DeliveryMode>(d => d.Available && producerIds.Contains(d.Producer.Id) && kinds.Contains(d.Kind), token);

            var deliveriesProducerIds = deliveriesMode.Select(c => c.Producer.Id).Distinct();
            var producerDistinctIds = producerIds.Distinct();
            if (deliveriesProducerIds.Count() != producerDistinctIds.Count())
            {
                var notFoundProducerIds = deliveriesProducerIds.Except(producerDistinctIds);
                var notFoundProducers = await _context.FindAsync<Domain.Producer>(c => notFoundProducerIds.Contains(c.Id), token);

                producers.AddRange(notFoundProducers.Select(c => new ProducerDeliveriesDto { Id = c.Id, Name = c.Name, Deliveries = null }));
            }

            foreach (var deliveriesGroup in deliveriesMode.GroupBy(c => c.Producer.Id))
            {
                var deliveries = new List<DeliveryDto>();
                var producer = new ProducerDeliveriesDto
                {
                    Id = deliveriesGroup.Key,
                    Name = deliveriesGroup.First().Producer.Name
                };

                foreach (var deliveryMode in deliveriesGroup)
                    deliveries.Add(GetDeliveriesForHours(currentDate, deliveryMode, deliveryMode.OpeningHours));

                producer.Deliveries = deliveries;
                producers.Add(producer);
            }

            if (deliveriesMode.All(dm => !dm.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return producers;

            return await GetNotCapedProducersDeliveriesAsync(producers, token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(Guid storeId, IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token)
        {
            var producers = new List<ProducerDeliveriesDto>();
            var agreements = await _context.FindAsync<Domain.Agreement>(d => d.Delivery.Available && producerIds.Contains(d.Delivery.Producer.Id) && d.Store.Id == storeId && d.Status == AgreementStatus.Accepted && kinds.Contains(d.Delivery.Kind), token);

            var agreementProducerIds = agreements.Select(c => c.Delivery.Producer.Id).Distinct();
            var producerDistinctIds = producerIds.Distinct();
            if (agreementProducerIds.Count() != producerDistinctIds.Count())
            {
                var notFoundProducerIds = agreementProducerIds.Except(producerDistinctIds);
                var notFoundProducers = await _context.FindAsync<Domain.Producer>(c => notFoundProducerIds.Contains(c.Id), token);

                producers.AddRange(notFoundProducers.Select(c => new ProducerDeliveriesDto { Id = c.Id, Name = c.Name, Deliveries = null }));
            }

            kinds ??= new List<DeliveryKind> {
                    DeliveryKind.ProducerToStore,
                    DeliveryKind.ExternalToStore
                };

            foreach (var agreementGroups in agreements.GroupBy(c => c.Delivery.Producer.Id))
            {
                var deliveries = new List<DeliveryDto>();
                var producer = new ProducerDeliveriesDto
                {
                    Id = agreementGroups.Key,
                    Name = agreementGroups.First().Delivery.Producer.Name
                };

                foreach (var agreement in agreementGroups)
                    deliveries.Add(GetDeliveriesForHours(currentDate, agreement.Delivery, agreement.SelectedHours));

                producer.Deliveries = deliveries;
                producers.Add(producer);
            }

            if (agreements.Select(a => a.Delivery).All(dm => !dm.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return producers;

            return await GetNotCapedProducersDeliveriesAsync(producers, token);
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetNotCapedProducersDeliveriesAsync(IEnumerable<ProducerDeliveriesDto> producersDeliveries, CancellationToken token)
        {
            var producerIds = producersDeliveries.Select(c => c.Id);
            var deliveryModeIds = producersDeliveries.SelectMany(c => c.Deliveries.Select(d => d.Id));

            var producers = await _context.FindAsync<Domain.Producer>(p => producerIds.Contains(p.Id), token);
            var deliveriesMode = await _context.FindAsync<Domain.DeliveryMode>(d => deliveryModeIds.Contains(d.Id), token);

            var producerDeliveriesHoursToCheck = new List<Tuple<Guid, Guid, DeliveryHourDto>>();
            foreach (var producer in producersDeliveries)
            {
                var deliveryModesToCheckMaxOrders = deliveriesMode.Where(dm => producer.Deliveries.Any(d => dm.MaxPurchaseOrdersPerTimeSlot.HasValue && dm.Id == d.Id));
                foreach (var deliveryMode in deliveryModesToCheckMaxOrders)
                {
                    var delivery = producer.Deliveries.FirstOrDefault(d => d.Id == deliveryMode.Id);
                    if (delivery == null)
                        continue;

                    producerDeliveriesHoursToCheck.AddRange(delivery.DeliveryHours.Select(dh => new Tuple<Guid, Guid, DeliveryHourDto>(producer.Id, delivery.Id, dh)));
                }
            }

            var results = await _capingDeliveriesService.GetCapingDeliveriesAsync(producerDeliveriesHoursToCheck, token);
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

        private DeliveryDto GetDeliveriesForHours(DateTimeOffset currentDate, Domain.DeliveryMode deliveryMode, IReadOnlyCollection<TimeSlotHour> openingHours)
        {
            return new DeliveryDto
            {
                Id = deliveryMode.Id,
                Kind = deliveryMode.Kind,
                Available = deliveryMode.Available,
                Address = deliveryMode.Address != null ? new AddressDto
                {
                    City = deliveryMode.Address.City,
                    Latitude = deliveryMode.Address.Latitude,
                    Line1 = deliveryMode.Address.Line1,
                    Line2 = deliveryMode.Address.Line2,
                    Longitude = deliveryMode.Address.Longitude,
                    Zipcode = deliveryMode.Address.Zipcode
                } : null,
                Name = deliveryMode.Name,
                DeliveryHours = GetAvailableDeliveryHours(openingHours, currentDate, deliveryMode.LockOrderHoursBeforeDelivery),
            };
        }

        private IEnumerable<DeliveryHourDto> GetAvailableDeliveryHours(IEnumerable<TimeSlotHour> openingHours, DateTimeOffset currentDate, int? lockOrderHoursBeforeDelivery)
        {
            var list = new List<DeliveryHourDto>();
            foreach (var openingHour in openingHours.OrderBy(oh => oh.Day))
            {
                var results = new List<DeliveryHourDto>();
                var increment = 0;
                while (results.Count < 2 && increment < 31)
                {
                    var diff = (int)openingHour.Day + increment - (int)currentDate.DayOfWeek;
                    var result = GetDeliveryHourIfMatch(currentDate, openingHour, diff, lockOrderHoursBeforeDelivery);
                    if (result != null)
                        results.Add(result);

                    increment += 7;
                }

                if (results.Any())
                    list.AddRange(results);
            }

            return list;
        }

        private DeliveryHourDto GetDeliveryHourIfMatch(DateTimeOffset currentDate, TimeSlotHour openingHour, int diff, int? lockOrderHoursBeforeDelivery)
        {
            var targetDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.From.Hours, openingHour.From.Minutes, openingHour.From.Seconds).AddDays(diff);
            if (currentDate.AddHours(lockOrderHoursBeforeDelivery ?? 0) >= targetDate)
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
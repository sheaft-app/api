using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Models;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Queries
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
            var deliveriesMode = await _context.FindAsync<DeliveryMode>(d => d.Available && producerIds.Contains(d.Producer.Id) && kinds.Contains(d.Kind), token);

            var deliveriesProducerIds = deliveriesMode.Select(c => c.Producer.Id).Distinct();
            var producerDistinctIds = producerIds.Distinct();
            if (deliveriesProducerIds.Count() != producerDistinctIds.Count())
            {
                var notFoundProducerIds = deliveriesProducerIds.Except(producerDistinctIds);
                var notFoundProducers = await _context.FindAsync<Producer>(c => notFoundProducerIds.Contains(c.Id), token);

                producers.AddRange(notFoundProducers.Select(c => new ProducerDeliveriesDto { Id = c.Id, Name = c.Name, Deliveries = null }));
            }

            var producerDeliveriesToCheck = new List<Tuple<Guid, Guid, DeliveryHourDto>>();
            foreach (var deliveriesGroup in deliveriesMode.GroupBy(c => c.Producer.Id))
            {
                var deliveries = new List<DeliveryDto>();
                var producer = new ProducerDeliveriesDto
                {
                    Id = deliveriesGroup.Key,
                    Name = deliveriesGroup.First().Producer.Name
                };

                foreach (var deliveryMode in deliveriesGroup)
                {
                    deliveries.Add(new DeliveryDto
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
                        DeliveryHours = GetAvailableDeliveryHours(deliveryMode.OpeningHours, currentDate, deliveryMode.LockOrderHoursBeforeDelivery),
                    });
                }

                producer.Deliveries = deliveries;

                var deliveryModesToCheckMaxOrders = deliveriesMode.Where(dm => deliveries.Any(d => dm.MaxPurchaseOrdersPerTimeSlot.HasValue && dm.Id == d.Id));
                if (deliveryModesToCheckMaxOrders.Any())
                {
                    foreach (var deliveryMode in deliveryModesToCheckMaxOrders)
                    {
                        var delivery = deliveries.FirstOrDefault(d => d.Id == deliveryMode.Id);
                        if (delivery == null)
                            continue;

                        producerDeliveriesToCheck.AddRange(delivery.DeliveryHours.Select(dh => new Tuple<Guid, Guid, DeliveryHourDto>(producer.Id, delivery.Id, dh)));
                    }
                }

                producers.Add(producer);
            }

            if (!producerDeliveriesToCheck.Any())
                return producers;

            return await GetNotCapedDeliveriesAsync(producers, deliveriesMode, producerDeliveriesToCheck, token);
        }

        private async Task<IEnumerable<ProducerDeliveriesDto>> GetNotCapedDeliveriesAsync(IEnumerable<ProducerDeliveriesDto> producers, IEnumerable<DeliveryMode> deliveriesMode, IEnumerable<Tuple<Guid, Guid, DeliveryHourDto>> producerDeliveriesToCheck, CancellationToken token)
        {
            var results = await _capingDeliveriesService.GetCapingDeliveriesAsync(producerDeliveriesToCheck, token);
            if (!results.Success)
                throw results.Exception;

            var filteredProducers = producers.ToList();
            foreach (var producer in filteredProducers)
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
            }

            return filteredProducers;
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(Guid storeId, IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token)
        {
            var list = new List<ProducerDeliveriesDto>();
            var agreements = await _context.FindAsync<Agreement>(d => d.Delivery.Available && producerIds.Contains(d.Delivery.Producer.Id) && d.Store.Id == storeId && d.Status == AgreementStatus.Accepted && kinds.Contains(d.Delivery.Kind), token);

            var agreementProducerIds = agreements.Select(c => c.Delivery.Producer.Id).Distinct();
            var producerDistinctIds = producerIds.Distinct();
            if (agreementProducerIds.Count() != producerDistinctIds.Count())
            {
                var notFoundProducerIds = agreementProducerIds.Except(producerDistinctIds);
                var producers = await _context.FindAsync<Producer>(c => notFoundProducerIds.Contains(c.Id), token);

                list.AddRange(producers.Select(c => new ProducerDeliveriesDto { Id = c.Id, Name = c.Name, Deliveries = null }));
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
                {
                    deliveries.Add(new DeliveryDto
                    {
                        Id = agreement.Delivery.Id,
                        Kind = agreement.Delivery.Kind,
                        Available = agreement.Delivery.Available,
                        Address = agreement.Delivery.Address != null ? new AddressDto
                        {
                            City = agreement.Delivery.Address.City,
                            Latitude = agreement.Delivery.Address.Latitude,
                            Line1 = agreement.Delivery.Address.Line1,
                            Line2 = agreement.Delivery.Address.Line2,
                            Longitude = agreement.Delivery.Address.Longitude,
                            Zipcode = agreement.Delivery.Address.Zipcode
                        } : null,
                        Name = agreement.Delivery.Name,
                        DeliveryHours = GetAvailableDeliveryHours(agreement.SelectedHours, currentDate, agreement.Delivery.LockOrderHoursBeforeDelivery),
                    });
                }

                producer.Deliveries = deliveries;
                list.Add(producer);
            }

            return list;
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
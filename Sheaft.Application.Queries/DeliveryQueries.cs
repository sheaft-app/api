using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Models.Dto;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class DeliveryQueries : IDeliveryQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public DeliveryQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<DeliveryModeDto> GetDelivery(Guid id, IRequestUser currentUser)
        {
            try
            {
                return _context.DeliveryModes
                        .Get(c => c.Id == id && c.Producer.Id == currentUser.CompanyId)
                        .ProjectTo<DeliveryModeDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DeliveryModeDto>().AsQueryable();
            }
        }

        public IQueryable<DeliveryModeDto> GetDeliveries(IRequestUser currentUser)
        {
            try
            {
                return _context.DeliveryModes
                        .Get(c => c.Producer.Id == currentUser.CompanyId)
                        .ProjectTo<DeliveryModeDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DeliveryModeDto>().AsQueryable();
            }
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                var list = new List<ProducerDeliveriesDto>();
                var deliveriesMode = await _context.GetAsync<DeliveryMode>(d => producerIds.Contains(d.Producer.Id) && kinds.Contains(d.Kind), token);

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
                            DeliveryHours = GetAvailableDeliveryHours(deliveryMode.OpeningHours, deliveryMode.LockOrderHoursBeforeDelivery, currentDate),
                        });
                    }

                    producer.Deliveries = deliveries;
                    list.Add(producer);
                }

                return list;
            }
            catch (Exception e)
            {
                return new List<ProducerDeliveriesDto>().AsQueryable();
            }
        }

        public async Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(Guid storeId, IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                var agreements = await _context.GetAsync<Agreement>(d => producerIds.Contains(d.Delivery.Producer.Id) && d.Store.Id == storeId && d.Status == AgreementStatusKind.Accepted && kinds.Contains(d.Delivery.Kind), token);

                var list = new List<ProducerDeliveriesDto>();
                kinds = kinds ?? new List<DeliveryKind> {
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
                            DeliveryHours = GetAvailableDeliveryHours(agreement.SelectedHours, agreement.Delivery.LockOrderHoursBeforeDelivery, currentDate),
                        });
                    }

                    producer.Deliveries = deliveries;
                    list.Add(producer);
                }

                return list;
            }
            catch (Exception e)
            {
                return new List<ProducerDeliveriesDto>().AsQueryable();
            }
        }

        private static IQueryable<DeliveryModeDto> GetAsDto(IQueryable<DeliveryMode> query)
        {
            return query
                .Select(a => new DeliveryModeDto
                {
                    Address = new AddressDto
                    {
                        City = a.Address.City,
                        Latitude = a.Address.Latitude,
                        Line1 = a.Address.Line1,
                        Line2 = a.Address.Line2,
                        Longitude = a.Address.Longitude,
                        Zipcode = a.Address.Zipcode
                    },
                    CreatedOn = a.CreatedOn,
                    Description = a.Description,
                    Id = a.Id,
                    Kind = a.Kind,
                    LockOrderHoursBeforeDelivery = a.LockOrderHoursBeforeDelivery,
                    Name = a.Name,
                    OpeningHours = a.OpeningHours.Select(oh => new TimeSlotDto
                    {
                        Day = oh.Day,
                        From = oh.From,
                        To = oh.To
                    }),
                    UpdatedOn = a.UpdatedOn
                });
        }

        private IEnumerable<DeliveryHourDto> GetAvailableDeliveryHours(IEnumerable<TimeSlotHour> openingHours, int lockOrderHoursBeforeDelivery, DateTimeOffset currentDate)
        {
            var list = new List<DeliveryHourDto>();
            foreach (var openingHour in openingHours.OrderBy(oh => oh.Day))
            {
                var diff = (int)openingHour.Day >= (int)currentDate.DayOfWeek ? (int)openingHour.Day - (int)currentDate.DayOfWeek : 0;
                var result = GetDeliveryHourIfMatch(currentDate, openingHour, diff, lockOrderHoursBeforeDelivery);
                if (result != null)
                    list.Add(result);

                diff = (int)openingHour.Day + 7 - (int)currentDate.DayOfWeek;
                result = GetDeliveryHourIfMatch(currentDate, openingHour, diff, lockOrderHoursBeforeDelivery);
                if (result != null)
                    list.Add(result);

                var increment = 14;
                while (result == null && increment < 31)
                {
                    diff = (int)openingHour.Day + increment - (int)currentDate.DayOfWeek;
                    result = GetDeliveryHourIfMatch(currentDate, openingHour, diff, lockOrderHoursBeforeDelivery);
                    if (result != null)
                        list.Add(result);

                    increment += 7;
                }
            }

            return list;
        }

        private DeliveryHourDto GetDeliveryHourIfMatch(DateTimeOffset currentDate, TimeSlotHour openingHour, int diff, int lockOrderHoursBeforeDelivery)
        {
            var targetDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.To.Hours, openingHour.To.Minutes, openingHour.To.Seconds).AddDays(diff);
            if (currentDate.AddHours(lockOrderHoursBeforeDelivery) > targetDate)
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
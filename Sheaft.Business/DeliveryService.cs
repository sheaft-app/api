using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Business
{
    public class DeliveryService : SheaftService, IDeliveryService
    {
        private readonly ITableService _tableService;

        public DeliveryService(
            ITableService tableService,
            ILogger<DeliveryService> logger) : base(logger)
        {
            _tableService = tableService;
        }

        public async Task<Result<bool>> ValidateCapedDeliveriesAsync(IReadOnlyCollection<OrderDelivery> orderDeliveries, CancellationToken token)
        {
            if (orderDeliveries.All(d => !d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return Success(true);

            var results = await _tableService.GetCapingDeliveriesInfosAsync(
                orderDeliveries.Where(d => d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue).Select(d =>
                    new Tuple<Guid, Guid, DeliveryHourDto>(
                        d.DeliveryMode.ProducerId,
                        d.DeliveryModeId,
                        new DeliveryHourDto
                        {
                            Day = d.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek,
                            ExpectedDeliveryDate = d.ExpectedDelivery.ExpectedDeliveryDate,
                            From = d.ExpectedDelivery.From,
                            To = d.ExpectedDelivery.To,
                        })
                ), token);

            if (!results.Succeeded)
                return Failure<bool>(results.Exception);

            var result = Success(true);
            foreach (var orderDelivery in orderDeliveries)
            {
                var delivery = results.Data.FirstOrDefault(d => d.DeliveryId == orderDelivery.Id
                                                                && d.ExpectedDate.Year == orderDelivery.ExpectedDelivery
                                                                    .ExpectedDeliveryDate.Year
                                                                && d.ExpectedDate.Month ==
                                                                orderDelivery.ExpectedDelivery.ExpectedDeliveryDate
                                                                    .Month
                                                                && d.ExpectedDate.Day == orderDelivery.ExpectedDelivery
                                                                    .ExpectedDeliveryDate.Day
                                                                && d.From == orderDelivery.ExpectedDelivery.From
                                                                && d.To == orderDelivery.ExpectedDelivery.To);

                if (delivery == null || delivery.Count < orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot)
                    continue;

                result = Failure<bool>(new ValidationException(
                    MessageKind.Order_CannotPay_Delivery_Max_PurchaseOrders_Reached,
                    orderDelivery.DeliveryMode.Producer.Name,
                    $"le {orderDelivery.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} entre {orderDelivery.ExpectedDelivery.From:hh\\hmm} et {orderDelivery.ExpectedDelivery.To:hh\\hmm}",
                    orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot));
                
                break;
            }

            return result;
        }
    }
}
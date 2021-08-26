using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Services
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

        public async Task<Result<bool>> ValidateCapedDeliveriesAsync(ICollection<OrderDelivery> orderDeliveries,
            CancellationToken token)
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
                            Day = d.Day,
                            ExpectedDeliveryDate = d.ExpectedDeliveryDate,
                            From = d.From,
                            To = d.To,
                        })
                ), token);

            if (!results.Succeeded)
                return Failure<bool>(results.Exception);

            var result = Success(true);
            foreach (var orderDelivery in orderDeliveries)
            {
                var delivery = results.Data.FirstOrDefault(d => d.DeliveryId == orderDelivery.DeliveryModeId
                                                                && d.ExpectedDate.Year ==
                                                                orderDelivery.ExpectedDeliveryDate.Year
                                                                && d.ExpectedDate.Month ==
                                                                orderDelivery.ExpectedDeliveryDate
                                                                    .Month
                                                                && d.ExpectedDate.Day ==
                                                                orderDelivery.ExpectedDeliveryDate.Day
                                                                && d.From == orderDelivery.From
                                                                && d.To == orderDelivery.To);

                if (delivery == null || delivery.Count < orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot)
                    continue;

                result = Failure<bool>(
                    $"Le nombre maximum de {orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot} commandes a été atteins pour le producteur {orderDelivery.DeliveryMode.Producer.Name} pour la date du {orderDelivery.ExpectedDeliveryDate:dd/MM/yyyy} entre {orderDelivery.From:hh\\hmm} et {orderDelivery.To:hh\\hmm}");

                break;
            }

            return result;
        }
    }
}
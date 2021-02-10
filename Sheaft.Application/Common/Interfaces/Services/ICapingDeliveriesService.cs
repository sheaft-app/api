using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface ICapingDeliveriesService
    {
        Task<Result<IEnumerable<CapingDeliveryDto>>> GetCapingDeliveriesAsync(IEnumerable<Tuple<Guid, Guid, DeliveryHourDto>> deliveries, CancellationToken token);
        Task<Result<CapingDeliveryDto>> GetCapingDeliveryAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, CancellationToken token);
        Task<Result> IncreaseProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, CancellationToken token);
        Task<Result> DecreaseProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, CancellationToken token);
    }
}
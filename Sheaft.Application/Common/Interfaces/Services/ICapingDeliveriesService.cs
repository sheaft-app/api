using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface ICapingDeliveriesService
    {
        Task<Result<IEnumerable<CapingDeliveryDto>>> GetCapingDeliveriesAsync(IEnumerable<Tuple<Guid, Guid, DeliveryHourDto>> deliveries, CancellationToken token);
        Task<Result<CapingDeliveryDto>> GetCapingDeliveryAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, CancellationToken token);
        Task<Result<bool>> IncreaseProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, CancellationToken token);
        Task<Result<bool>> DecreaseProducerDeliveryCountAsync(Guid producerId, Guid deliveryId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, int maxPurchaseOrders, CancellationToken token);
    }
}
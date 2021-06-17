using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;

namespace Sheaft.Application.Interfaces.Business
{
    public interface IDeliveryBatchService
    {
        Task<IEnumerable<AvailableDeliveryBatchDto>> GetAvailableDeliveryBatchesAsync(Guid producerId, bool includeProcessingPurchaseOrders,
            CancellationToken token);
    }
}
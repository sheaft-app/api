using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;

namespace Sheaft.Application.Interfaces.Business
{
    public interface IPickingService
    {
        Task<IEnumerable<AvailablePickingDto>> GetAvailablePickingsAsync(Guid producerId, bool includePendingPurchaseOrders,
            CancellationToken token);
    }
}
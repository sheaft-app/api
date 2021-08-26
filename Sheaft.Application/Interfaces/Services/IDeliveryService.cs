using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Services
{
    public interface IDeliveryService
    {
        Task<Result<bool>> ValidateCapedDeliveriesAsync(ICollection<OrderDelivery> orderDeliveries, CancellationToken token);
    }
}
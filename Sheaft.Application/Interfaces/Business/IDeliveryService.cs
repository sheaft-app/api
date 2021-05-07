using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Business
{
    public interface IDeliveryService
    {
        Task<Result<bool>> ValidateCapedDeliveriesAsync(ICollection<OrderDelivery> orderDeliveries, CancellationToken token);
    }
}
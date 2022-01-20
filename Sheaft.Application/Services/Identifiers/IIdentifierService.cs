using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Identifiers
{
    public interface IIdentifierService
    {
        Task<Result<int>> GetNextPurchaseOrderReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<Result<int>> GetNextDeliveryReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<Result<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token);
    }
}

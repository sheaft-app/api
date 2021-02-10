using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface IIdentifierService
    {
        Task<Result<string>> GetNextOrderReferenceAsync(CancellationToken token);
        Task<Result<string>> GetNextPurchaseOrderReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<Result<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<Result<string>> GetNextSponsoringCode(CancellationToken token);
    }
}

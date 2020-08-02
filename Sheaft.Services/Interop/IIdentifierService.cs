using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services.Interop
{
    public interface IIdentifierService
    {
        string RemoveDiacritics(string value);
        string NormalizeIdentifier(string name, string pattern);
        Task<CommandResult<string>> GetNextOrderReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<CommandResult<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token);
        Task<CommandResult<string>> GetNextSponsoringCode(CancellationToken token);
    }
}

using Sheaft.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services.Interop
{
    public interface IEmailService
    {
        Task<CommandResult<bool>> SendTemplatedEmailAsync<T>(string toEmail, string toName, string templateId, T datas, CancellationToken token);
    }
}
using Sheaft.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IEmailService
    {
        Task<Result<bool>> SendTemplatedEmailAsync<T>(string toEmail, string toName, string templateId, T datas, CancellationToken token);
        Task<Result<bool>> SendEmailAsync(string toEmail, string toName, string subject, string content, bool isHtml, CancellationToken token);
    }
}
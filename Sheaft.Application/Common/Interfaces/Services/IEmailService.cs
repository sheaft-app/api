using Sheaft.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IEmailService
    {
        Task<Result<bool>> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject, string templateId, T datas, bool isHtml, CancellationToken token);
        Task<Result<bool>> SendEmailAsync(string toEmail, string toName, string subject, string content, bool isHtml, CancellationToken token);
    }
}
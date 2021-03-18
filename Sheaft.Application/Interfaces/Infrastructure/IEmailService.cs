using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject, string templateId, T datas, bool isHtml, CancellationToken token);
        Task<Result> SendEmailAsync(string toEmail, string toName, string subject, string content, bool isHtml, CancellationToken token);
    }
}
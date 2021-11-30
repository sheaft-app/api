using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Notifications
{
    public interface IEmailService
    {
        Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject, string templateId,
            T data, bool isHtml, CancellationToken token);

        Task<Result> SendEmailAsync(string toEmail, string toName, string subject, string content, bool isHtml,
            CancellationToken token);
    }
}
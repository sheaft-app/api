using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject, string templateId,
            T data, bool isHtml, CancellationToken token);

        Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject, string templateId,
            T data, IEnumerable<EmailAttachmentDto> attachments, bool isHtml, CancellationToken token);

        Task<Result> SendEmailAsync(string toEmail, string toName, string subject, string content, bool isHtml,
            CancellationToken token);
    }
}
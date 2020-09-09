using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Sheaft.Core;
using Sheaft.Interop.Enums;
using Sheaft.Options;
using Sheaft.Services.Interop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendgridOptions _sendgridOptions;
        private readonly ISendGridClient _sendgrid;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptionsSnapshot<SendgridOptions> sendgridOptions, 
            ILogger<EmailService> logger, 
            ISendGridClient sendgrid)
        {
            _logger = logger;
            _sendgridOptions = sendgridOptions.Value;
            _sendgrid = sendgrid;
        }

        public async Task<Result<bool>> SendTemplatedEmailAsync<T>(string toEmail, string toName, string templateId, T datas, CancellationToken token)
        {
            try
            {
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress(_sendgridOptions.Sender.Email, _sendgridOptions.Sender.Name));

                var recipients = new List<EmailAddress>
                {
                    new EmailAddress(toEmail, toName)
                };

                msg.AddTos(recipients);
                msg.SetTemplateId(templateId);
                msg.SetTemplateData(datas);

                var response = await _sendgrid.SendEmailAsync(msg);
                if ((int)response.StatusCode >= 400)
                    return new Result<bool>(false, MessageKind.EmailProvider_SendEmail_Failure, await response.Body.ReadAsStringAsync());

                return new Result<bool>(true);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"{nameof(EmailService.SendTemplatedEmailAsync)} - {e.Message}");
                return new Result<bool>(e);
            }
        }
    }
}

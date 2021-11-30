using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;
using Sheaft.Application.Configurations;
using Sheaft.Application.Notifications;
using Sheaft.Domain.Common;
using Sheaft.Infrastructure.Extensions;

namespace Sheaft.Infrastructure.Notifications
{
    internal class EmailService : IEmailService
    {
        private readonly IAmazonSimpleEmailService _mailer;
        private readonly MailerConfiguration _mailerConfiguration;

        public EmailService(
            IOptionsSnapshot<MailerConfiguration> mailerOptions,
            IAmazonSimpleEmailService mailer,
            ILogger<EmailService> logger)
        {
            _mailerConfiguration = mailerOptions.Value;
            _mailer = mailer;
        }

        public async Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject,
            string templateId, T data, bool isHtml, CancellationToken token)
        {
            var msg = new SendEmailRequest();
            msg.Destination = new Destination
            {
                ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
            };

            msg.Source = $"{_mailerConfiguration.Sender.Name}<{_mailerConfiguration.Sender.Email}>";
            msg.ReturnPath = _mailerConfiguration.Bounces;
            msg.Message = new Message
            {
                Subject = new Content(subject)
            };

            var content = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data);
            if (isHtml)
                msg.Message.Body = new Body {Html = new Content(content)};
            else
                msg.Message.Body = new Body {Text = new Content(content)};

            if (_mailerConfiguration.SkipSending)
                return Result.Success();

            var response = await _mailer.SendEmailAsync(msg, token);
            if ((int) response.HttpStatusCode >= 400)
                return Result.Failure("Une erreur est survenue pendant l'envoi de l'email.");

            return Result.Success();
        }

        public async Task<Result> SendEmailAsync(string toEmail, string toName, string subject, string content,
            bool isHtml, CancellationToken token)
        {
            var msg = new SendEmailRequest();
            msg.Destination = new Destination
            {
                ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
            };

            msg.Source = $"{_mailerConfiguration.Sender.Name}<{_mailerConfiguration.Sender.Email}>";
            msg.ReturnPath = _mailerConfiguration.Bounces;
            msg.Message = new Message
            {
                Subject = new Content(subject)
            };

            if (isHtml)
                msg.Message.Body = new Body {Html = new Content(content)};
            else
                msg.Message.Body = new Body {Text = new Content(content)};

            if (_mailerConfiguration.SkipSending)
                return Result.Success();

            var response = await _mailer.SendEmailAsync(msg, token);
            if ((int) response.HttpStatusCode >= 400)
                return Result.Failure("Une erreur est survenue pendant l'envoi de l'email.");

            return Result.Success();
        }
    }
}
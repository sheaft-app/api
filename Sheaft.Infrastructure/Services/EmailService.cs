using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;
using Razor.Templating.Core;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Extensions;
using Sheaft.Options;

namespace Sheaft.Infrastructure.Services
{
    public class EmailService : SheaftService, IEmailService
    {
        private readonly IAmazonSimpleEmailService _mailer;
        private readonly MailerOptions _mailerOptions;

        public EmailService(
            IOptionsSnapshot<MailerOptions> mailerOptions,
            IAmazonSimpleEmailService mailer,
            ILogger<EmailService> logger)
            : base(logger)
        {
            _mailerOptions = mailerOptions.Value;
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

            msg.Source = $"{_mailerOptions.Sender.Name}<{_mailerOptions.Sender.Email}>";
            msg.ReturnPath = _mailerOptions.Bounces;
            msg.Message = new Message
            {
                Subject = new Content(subject)
            };

            var content = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data);
            if (isHtml)
                msg.Message.Body = new Body {Html = new Content(content)};
            else
                msg.Message.Body = new Body {Text = new Content(content)};

            if (_mailerOptions.SkipSending)
                return Success();
            
            var response = await _mailer.SendEmailAsync(msg, token);
            if ((int) response.HttpStatusCode >= 400)
                return Failure(MessageKind.EmailProvider_SendEmail_Failure,
                    string.Join(";", response.ResponseMetadata.Metadata));

            return Success();
        }

        public async Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject,
            string templateId, T data, IEnumerable<EmailAttachmentDto> attachments, bool isHtml, CancellationToken token)
        {
            var msg = new SendRawEmailRequest();
            using (var messageStream = new MemoryStream())
            {
                var message = new MimeMessage();
                
                var content = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data);
                var builder = isHtml ? new BodyBuilder {HtmlBody = content} : new BodyBuilder {TextBody = content};

                message.To.Add(new MailboxAddress(toName, toEmail));
                message.From.Add(new MailboxAddress(_mailerOptions.Sender.Name, _mailerOptions.Sender.Email));
                message.Subject = subject;

                foreach (var attachment in attachments)
                    builder.Attachments.Add(attachment.Name, attachment.Content);
               
                message.Body = builder.ToMessageBody();
                await message.WriteToAsync(messageStream, token);

                msg.RawMessage = new RawMessage() {Data = messageStream};
            }
            
            if (_mailerOptions.SkipSending)
                return Success();
            
            var response = await _mailer.SendRawEmailAsync(msg, token);
            if ((int) response.HttpStatusCode >= 400)
                return Failure(MessageKind.EmailProvider_SendEmail_Failure,
                    string.Join(";", response.ResponseMetadata.Metadata));

            return Success();
        }

        public async Task<Result> SendEmailAsync(string toEmail, string toName, string subject, string content,
            bool isHtml, CancellationToken token)
        {
                var msg = new SendEmailRequest();
                msg.Destination = new Destination
                {
                    ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
                };

                msg.Source = $"{_mailerOptions.Sender.Name}<{_mailerOptions.Sender.Email}>";
                msg.ReturnPath = _mailerOptions.Bounces;
                msg.Message = new Message
                {
                    Subject = new Content(subject)
                };

                if (isHtml)
                    msg.Message.Body = new Body {Html = new Content(content)};
                else
                    msg.Message.Body = new Body {Text = new Content(content)};

                if (_mailerOptions.SkipSending)
                    return Success();
                
                var response = await _mailer.SendEmailAsync(msg, token);
                if ((int) response.HttpStatusCode >= 400)
                    return Failure(MessageKind.EmailProvider_SendEmail_Failure,
                        string.Join(";", response.ResponseMetadata.Metadata));

                return Success();
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;
using Sheaft.Application.Configurations;
using Sheaft.Application.Services;
using Sheaft.Domain.Common;

namespace Sheaft.Infrastructure.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IAmazonSimpleEmailService _mailer;
        private readonly ILogger<EmailService> _logger;
        private readonly MailerConfiguration _mailerConfiguration;

        public EmailService(
            IOptionsSnapshot<MailerConfiguration> mailerOptions,
            IAmazonSimpleEmailService mailer,
            ILogger<EmailService> logger)
        {
            _mailerConfiguration = mailerOptions.Value;
            _mailer = mailer;
            _logger = logger;
        }

        public async Task<Result> SendTemplatedEmailAsync<T>(string toEmail, string toName, string subject,
            string templateId, T data, bool isHtml, CancellationToken token)
        {
            var content = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data);
            var msg = new SendEmailRequest
            {
                Destination = new Destination
                {
                    ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
                },
                Source = $"{_mailerConfiguration.Sender.Name}<{_mailerConfiguration.Sender.Email}>",
                ReturnPath = _mailerConfiguration.Bounces,
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = isHtml ? new Body {Html = new Content(content)} : new Body {Text = new Content(content)}
                }
            };

            if (_mailerConfiguration.SkipSending)
                return Result.Success();

            var response = await _mailer.SendEmailAsync(msg, token);
            return (int) response.HttpStatusCode >= 400 
                ? Result.Error("Une erreur est survenue pendant l'envoi de l'email.") 
                : Result.Success();
        }

        public async Task<Result> SendEmailAsync(string toEmail, string toName, string subject, string content,
            bool isHtml, CancellationToken token)
        {
            var msg = new SendEmailRequest
            {
                Destination = new Destination
                {
                    ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
                },
                Source = $"{_mailerConfiguration.Sender.Name}<{_mailerConfiguration.Sender.Email}>",
                ReturnPath = _mailerConfiguration.Bounces,
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = isHtml ? new Body {Html = new Content(content)} : new Body {Text = new Content(content)}
                }
            };

            if (_mailerConfiguration.SkipSending)
                return Result.Success();

            var response = await _mailer.SendEmailAsync(msg, token);
            return (int) response.HttpStatusCode >= 400 
                ? Result.Error("Une erreur est survenue pendant l'envoi de l'email.") 
                : Result.Success();
        }
    }
    
    internal static class Base64Extensions
    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
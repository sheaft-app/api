using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;
using Sheaft.Application;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Services;

internal class EmailService : IEmailService
{
    private readonly IAmazonSimpleEmailService _mailer;
    private readonly IMailerSettings _mailerSettings;

    public EmailService(
        IOptionsSnapshot<IMailerSettings> mailerOptions,
        IAmazonSimpleEmailService mailer)
    {
        _mailerSettings = mailerOptions.Value;
        _mailer = mailer;
    }

    public async Task<Result> SendTemplatedEmail<T>(string toEmail, string toName, string subject,
        string templateId, T data, bool isHtml, CancellationToken token)
    {
        var content = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data);
        var msg = new SendEmailRequest
        {
            Destination = new Destination
            {
                ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
            },
            Source = $"{_mailerSettings.Sender.Name}<{_mailerSettings.Sender.Email}>",
            ReturnPath = _mailerSettings.Bounces,
            Message = new Message
            {
                Subject = new Content(subject),
                Body = isHtml ? new Body {Html = new Content(content)} : new Body {Text = new Content(content)}
            }
        };

        if (_mailerSettings.SkipSending)
            return Result.Success();

        var response = await _mailer.SendEmailAsync(msg, token);
        return (int) response.HttpStatusCode >= 400
            ? Result.Failure(ErrorKind.BadRequest, "emailing.error", response.MessageId)
            : Result.Success();
    }

    public async Task<Result> SendEmail(string toEmail, string toName, string subject, string content,
        bool isHtml, CancellationToken token)
    {
        var msg = new SendEmailRequest
        {
            Destination = new Destination
            {
                ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
            },
            Source = $"{_mailerSettings.Sender.Name}<{_mailerSettings.Sender.Email}>",
            ReturnPath = _mailerSettings.Bounces,
            Message = new Message
            {
                Subject = new Content(subject),
                Body = isHtml ? new Body {Html = new Content(content)} : new Body {Text = new Content(content)}
            }
        };

        if (_mailerSettings.SkipSending)
            return Result.Success();

        var response = await _mailer.SendEmailAsync(msg, token);
        return (int) response.HttpStatusCode >= 400
            ? Result.Failure(ErrorKind.BadRequest, "emailing.error", response.MessageId)
            : Result.Success();
    }
}
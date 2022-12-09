﻿using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;
using Sheaft.Application;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Services;

internal class EmailingService : IEmailingService
{
    private readonly IAmazonSimpleEmailService _mailer;
    private readonly IEmailingSettings _emailingSettings;

    public EmailingService(
        IOptionsSnapshot<IEmailingSettings> mailerOptions,
        IAmazonSimpleEmailService mailer)
    {
        _emailingSettings = mailerOptions.Value;
        _mailer = mailer;
    }

    public async Task<Result> SendTemplatedEmail<T>(string toEmail, string toName, string subject,
        string templateId, T data, bool isHtml, CancellationToken token)
    {
        var content = await RazorTemplateEngine.RenderAsync($"~/Templates/{templateId}.cshtml", data ?? throw new ArgumentNullException(nameof(data)));
        var msg = new SendEmailRequest
        {
            Destination = new Destination
            {
                ToAddresses = new List<string> {$"=?UTF-8?B?{toName.Base64Encode()}?= <{toEmail}>"}
            },
            Source = $"{_emailingSettings.Sender.Name}<{_emailingSettings.Sender.Email}>",
            ReturnPath = _emailingSettings.Bounces,
            Message = new Message
            {
                Subject = new Content(subject),
                Body = isHtml ? new Body {Html = new Content(content)} : new Body {Text = new Content(content)}
            }
        };

        if (_emailingSettings.SkipSending)
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
            Source = $"{_emailingSettings.Sender.Name}<{_emailingSettings.Sender.Email}>",
            ReturnPath = _emailingSettings.Bounces,
            Message = new Message
            {
                Subject = new Content(subject),
                Body = isHtml ? new Body {Html = new Content(content)} : new Body {Text = new Content(content)}
            }
        };

        if (_emailingSettings.SkipSending)
            return Result.Success();

        var response = await _mailer.SendEmailAsync(msg, token);
        return (int) response.HttpStatusCode >= 400
            ? Result.Failure(ErrorKind.BadRequest, "emailing.error", response.MessageId)
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
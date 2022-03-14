namespace Sheaft.Domain;

public interface IEmailService
{
    Task<Result> SendTemplatedEmail<T>(string toEmail, string toName, string subject, string templateId,
        T data, bool isHtml, CancellationToken token);

    Task<Result> SendEmail(string toEmail, string toName, string subject, string content, bool isHtml,
        CancellationToken token);
}
namespace Sheaft.Domain;

public interface ISignalrService
{
    Task<Result> SendNotificationTo<T>(string userIdentifier, string eventName, T content, CancellationToken token);
}
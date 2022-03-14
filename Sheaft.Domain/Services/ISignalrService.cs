namespace Sheaft.Domain;

public interface ISignalrService
{
    Task<Result> SendNotificationTo<T>(ProfileId profileIdentifier, string eventName, T content, CancellationToken token);
}
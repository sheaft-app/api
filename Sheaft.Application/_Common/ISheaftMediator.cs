using Sheaft.Domain;

namespace Sheaft.Application;

public interface ISheaftMediator
{
    void Publish(IDomainEvent notification, string jobName = null);
    void Post(ICommand<Result> command, string jobName = null);
    void Post<T>(ICommand<Result<T>> command, string jobName = null);
    void Schedule(ICommand<Result> command, TimeSpan delay, string jobName = null);
    void Schedule<T>(ICommand<Result<T>> command, TimeSpan delay, string jobName = null);
    Task<Result> Execute(ICommand<Result> command, CancellationToken token);
    Task<Result<T>> Execute<T>(ICommand<Result<T>> command, CancellationToken token);
    Task<Result> Query(IQuery<Result> command, CancellationToken token);
    Task<Result<T>> Query<T>(IQuery<Result<T>> command, CancellationToken token);
}
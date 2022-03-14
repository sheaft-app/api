using Sheaft.Domain;

namespace Sheaft.Application;

public interface ISheaftDispatcher
{
    Task<Result> Execute(string jobName, ICommand<Result> data, CancellationToken token);
    Task<Result<T>> Execute<T>(string jobName, ICommand<Result<T>> data, CancellationToken token);
    Task Execute(string jobName, IDomainEvent data, CancellationToken token);
}
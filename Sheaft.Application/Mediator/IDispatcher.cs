using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public interface IDispatcher
    {
        Task Execute(string jobName, IIntegrationEvent data, CancellationToken token);
        Task<Result<T>> Execute<T>(string jobName, ICommand<Result<T>> data, CancellationToken token);
        Task<Result> Execute(string jobName, ICommand<Result> data, CancellationToken token);
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;

namespace Sheaft.Application.Mediator
{
    public interface IDispatcher
    {
        Task Execute(string jobName, DomainEvent data, CancellationToken token);
        Task<Result<T>> Execute<T>(string jobName, IRequest<Result<T>> data, CancellationToken token);
        Task<Result> Execute(string jobName, IRequest<Result> data, CancellationToken token);
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Interfaces.Services
{
    public interface ISheaftDispatcher
    {
        Task Execute(string jobname, DomainEvent data, CancellationToken token);
        Task<Result<T>> Execute<T>(string jobname, IRequest<Result<T>> data, CancellationToken token);
        Task<Result> Execute(string jobname, IRequest<Result> data, CancellationToken token);
    }
}
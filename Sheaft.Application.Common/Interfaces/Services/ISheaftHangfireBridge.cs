using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface ISheaftHangfireBridge
    {
        Task Execute(string jobname, DomainEvent data, CancellationToken token);
        Task<Result<T>> Execute<T>(string jobname, IRequest<Result<T>> data, CancellationToken token);
        Task<Result> Execute(string jobname, IRequest<Result> data, CancellationToken token);
    }
}
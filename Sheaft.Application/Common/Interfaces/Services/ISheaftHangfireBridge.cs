using MediatR;
using Sheaft.Core;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Interop
{
    public interface ISheaftHangfireBridge
    {
        Task Execute(string jobname, DomainEvent data, CancellationToken token);
        Task<Result<T>> Execute<T>(string jobname, IRequest<Result<T>> data, CancellationToken token);
    }
}
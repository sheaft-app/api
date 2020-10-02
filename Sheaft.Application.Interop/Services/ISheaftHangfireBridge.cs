using MediatR;
using Sheaft.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface ISheaftHangfireBridge
    {
        Task Execute(string jobname, INotification data, CancellationToken token);
        Task<Result<T>> Execute<T>(string jobname, IRequest<Result<T>> data, CancellationToken token);
    }
}
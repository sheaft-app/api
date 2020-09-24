using MediatR;
using Sheaft.Core;
using Sheaft.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface ISheaftMediatr
    {
        Task Post<T>(IRequest<Result<T>> data, CancellationToken token);
        Task Post(INotification data, CancellationToken token);
        Task Post(Job job, CancellationToken token);
        Task<Result<T>> Process<T>(IRequest<Result<T>> data, CancellationToken token);
        Task<Result<U>> Process<T, U>(string data, CancellationToken token)
            where T : IRequest<Result<U>>;
        Task Process<T>(string data, CancellationToken token)
            where T : INotification;
        Task Process(INotification data, CancellationToken token);
    }
}
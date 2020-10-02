using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IQueueService
    {
        Task ProcessEventAsync<T>(T item, CancellationToken token) where T: INotification;
        Task ProcessCommandAsync<T>(T item, CancellationToken token) where T: IBaseRequest;
    }
}
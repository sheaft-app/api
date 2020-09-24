using MediatR;
using Sheaft.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IQueueService
    {
        Task ProcessEventAsync<T>(T item, CancellationToken token) where T: INotification;
        Task ProcessCommandAsync<T>(T item, CancellationToken token) where T: IBaseRequest;
        Task InsertJobToProcessAsync(Job entity, CancellationToken token);
        Task<IEnumerable<string>> GetExistingQueues(int take, int skip, CancellationToken token);
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Interop
{
    public class SheaftMediatr : ISheaftMediatr
    {
        private readonly IMediator _mediatr;
        private readonly IQueueService _queueService;

        public SheaftMediatr(
            IMediator mediatr,
            IQueueService queueService)
        {
            _mediatr = mediatr;
            _queueService = queueService;
        }

        public async Task Post<T>(IRequest<Result<T>> data, CancellationToken token)
        {
            await _queueService.ProcessCommandAsync(data, token);
        }

        public async Task Post(INotification data, CancellationToken token)
        {
            await _queueService.ProcessEventAsync(data, token);
        }

        public async Task Post(Job job, CancellationToken token)
        {
            await _queueService.InsertJobToProcessAsync(job, token);
        }

        public async Task<Result<T>> Process<T>(IRequest<Result<T>> data, CancellationToken token)
        {
            return await _mediatr.Send(data, token);
        }

        public async Task Process(INotification data, CancellationToken token)
        {
            await _mediatr.Publish(data, token);
        }

        public async Task<Result<U>> Process<T, U>(string data, CancellationToken token) where T : IRequest<Result<U>>
        {
            var command = JsonConvert.DeserializeObject<T>(data);
            return await Process(command, token);
        }

        public async Task Process<T>(string data, CancellationToken token) where T : INotification
        {
            var command = JsonConvert.DeserializeObject<T>(data);
            await Process(command, token);
        }
    }
}
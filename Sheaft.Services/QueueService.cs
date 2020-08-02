using Azure.Storage.Queues;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Models;
using Sheaft.Options;
using Sheaft.Services.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services
{
    public class QueueService : IQueueService
    {
        private readonly StorageOptions _storageOptions;
        private readonly ILogger<QueueService> _logger;

        public QueueService(IOptionsSnapshot<StorageOptions> storageOptions, ILogger<QueueService> logger)
        {
            _logger = logger;
            _storageOptions = storageOptions.Value;
        }

        public async Task ProcessEventAsync<T>(string queueName, T item, CancellationToken token) where T : INotification
        {
            await InsertIntoQueueAsync(queueName, item, token);
        }

        public async Task ProcessCommandAsync<T>(string queueName, T item, CancellationToken token) where T : IBaseRequest
        {
            await InsertIntoQueueAsync(queueName, item, token);
        }

        public async Task InsertJobToProcessAsync(Job entity, CancellationToken token)
        {
            await InsertIntoQueueAsync(entity.Queue, entity.Command, token);
        }

        private async Task InsertIntoQueueAsync<T>(string queueName, T item, CancellationToken token)
        {
            await InsertIntoQueueAsync(queueName, JsonConvert.SerializeObject(item), token);
        }

        private async Task InsertIntoQueueAsync(string queueName, string item, CancellationToken token)
        {
            try
            {
                var queueClient = new QueueClient(_storageOptions.ConnectionString, queueName);

                await queueClient.CreateIfNotExistsAsync(cancellationToken: token);
                await queueClient.SendMessageAsync(item.Base64Encode(), token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Infrastructure.Services
{
    public class QueueService : IQueueService
    {
        private readonly ILogger<QueueService> _logger;

        public QueueService(
            ILogger<QueueService> logger)
        {
            _logger = logger;
        }

        public async Task ProcessEventAsync<T>(T item, CancellationToken token) where T : INotification
        {
            await InsertIntoQueueAsync("events", item, token);
        }

        public async Task ProcessCommandAsync<T>(T item, CancellationToken token) where T : IBaseRequest
        {
            await InsertIntoQueueAsync("commands", item, token);
        }

        private async Task InsertIntoQueueAsync<T>(string queueName, T item, CancellationToken token)
        {
            await InsertIntoQueueAsync(queueName, JsonConvert.SerializeObject(item), token);
        }

        private async Task InsertIntoQueueAsync(string queueName, string item, CancellationToken token)
        {
            try
            {
            //    var managementClient = new ManagementClient(_serviceBusOptions.ConnectionString);
            //    if (!(await managementClient.QueueExistsAsync(queueName)))
            //        await managementClient.CreateQueueAsync(new QueueDescription(queueName), token);

            //    var queueClient = new QueueClient(_serviceBusOptions.ConnectionString, queueName);
            //    await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(item)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}

using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Domain.Models;
using Sheaft.Options;
using Sheaft.Application.Interop;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Infrastructure.Services
{
    public class QueueService : IQueueService
    {
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly ILogger<QueueService> _logger;

        public QueueService(
            IOptionsSnapshot<ServiceBusOptions> serviceBusOptions,
            ILogger<QueueService> logger)
        {
            _logger = logger;
            _serviceBusOptions = serviceBusOptions.Value;
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
                var managementClient = new ManagementClient(_serviceBusOptions.ConnectionString);
                if (!(await managementClient.QueueExistsAsync(queueName)))
                    await managementClient.CreateQueueAsync(new QueueDescription(queueName), token);

                var queueClient = new QueueClient(_serviceBusOptions.ConnectionString, queueName);
                await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(item)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}

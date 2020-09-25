using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Functions
{
    public class TransferRefundFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public TransferRefundFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("RefreshTransferRefundStatusCommand")]
        public async Task RefreshTransferRefundStatusCommandAsync([ServiceBusTrigger(RefreshTransferRefundStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayoutRefundStatusCommand, TransactionStatus>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("TransferRefundFailedEvent")]
        public async Task TransferRefundFailedEventAsync([ServiceBusTrigger(TransferRefundFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<TransferRefundFailedEvent>(message, token);
        }

        [FunctionName("TransferRefundSucceededEvent")]
        public async Task TransferRefundSucceededEventAsync([ServiceBusTrigger(TransferRefundSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<TransferRefundSucceededEvent>(message, token);
        }
    }
}

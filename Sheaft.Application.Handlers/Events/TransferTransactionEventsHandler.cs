using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{

    public class TransferTransactionEventsHandler :
        INotificationHandler<TransferFailedEvent>,
        INotificationHandler<TransferSucceededEvent>,
        INotificationHandler<RefundTransferFailedEvent>,
        INotificationHandler<RefundTransferSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public TransferTransactionEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(TransferFailedEvent transferEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TransferSucceededEvent transferEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(RefundTransferFailedEvent refundTransferEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(RefundTransferSucceededEvent refundTransferEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
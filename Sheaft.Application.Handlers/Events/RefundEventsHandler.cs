using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class RefundEventsHandler :
        INotificationHandler<PayinRefundFailedEvent>,
        INotificationHandler<PayinRefundSucceededEvent>,
        INotificationHandler<TransferRefundFailedEvent>,
        INotificationHandler<TransferRefundSucceededEvent>,
        INotificationHandler<PayoutRefundFailedEvent>,
        INotificationHandler<PayoutRefundSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public RefundEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(PayinRefundFailedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(PayinRefundSucceededEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(TransferRefundFailedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(TransferRefundSucceededEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(PayoutRefundFailedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(PayoutRefundSucceededEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class PayinRefundEventsHandler :
        INotificationHandler<PayinRefundFailedEvent>,
        INotificationHandler<PayinRefundSucceededEvent>,
        INotificationHandler<CreatePayinRefundFailedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public PayinRefundEventsHandler(IAppDbContext context, IEmailService emailService)
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

        public Task Handle(CreatePayinRefundFailedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
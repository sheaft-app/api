using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class PayoutEventsHandler :
        INotificationHandler<PayoutFailedEvent>,
        INotificationHandler<PayoutSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public PayoutEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(PayoutFailedEvent payoutEvent, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(PayoutSucceededEvent payoutEvent, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
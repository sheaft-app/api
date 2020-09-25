using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class PayinEventsHandler :
        INotificationHandler<PayinFailedEvent>,
        INotificationHandler<PayinSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public PayinEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(PayinFailedEvent payinEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PayinSucceededEvent payinEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
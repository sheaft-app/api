using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{

    public class PayinTransactionEventsHandler :
        INotificationHandler<PayinFailedEvent>,
        INotificationHandler<PayinSucceededEvent>,
        INotificationHandler<RefundPayinFailedEvent>,
        INotificationHandler<RefundPayinSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public PayinTransactionEventsHandler(IAppDbContext context, IEmailService emailService)
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

        public Task Handle(RefundPayinFailedEvent refundPayinEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(RefundPayinSucceededEvent refundPayinEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
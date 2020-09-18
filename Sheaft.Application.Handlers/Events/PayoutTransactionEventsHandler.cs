using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{

    public class PayoutTransactionEventsHandler :
        INotificationHandler<PayoutFailedEvent>,
        INotificationHandler<PayoutSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public PayoutTransactionEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(PayoutFailedEvent payoutEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PayoutSucceededEvent payoutEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
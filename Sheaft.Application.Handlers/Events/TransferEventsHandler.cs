using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class TransferEventsHandler :
    INotificationHandler<TransferFailedEvent>,
    INotificationHandler<TransferSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public TransferEventsHandler(IAppDbContext context, IEmailService emailService)
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
    }
}
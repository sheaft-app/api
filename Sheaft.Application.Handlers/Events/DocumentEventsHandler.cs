using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{

    public class DocumentEventsHandler :
        INotificationHandler<DocumentFailedEvent>,
        INotificationHandler<DocumentOutdatedEvent>,
        INotificationHandler<DocumentSucceededEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public DocumentEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(DocumentFailedEvent docEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(DocumentOutdatedEvent docEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(DocumentSucceededEvent docEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
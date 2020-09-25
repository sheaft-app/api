using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class DocumentEventsHandler :
        INotificationHandler<DocumentRefusedEvent>,
        INotificationHandler<DocumentOutdatedEvent>,
        INotificationHandler<DocumentValidatedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public DocumentEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(DocumentRefusedEvent docEvent, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(DocumentOutdatedEvent docEvent, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(DocumentValidatedEvent docEvent, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
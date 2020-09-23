using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class ProducerEventsHandler :
        INotificationHandler<ProducerNotConfiguredEvent>,
        INotificationHandler<ProducerDocumentsNotCreatedEvent>,
        INotificationHandler<ProducerDocumentsNotReviewedEvent>,
        INotificationHandler<ProducerDocumentsNotValidatedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public ProducerEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(ProducerNotConfiguredEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(ProducerDocumentsNotCreatedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(ProducerDocumentsNotReviewedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(ProducerDocumentsNotValidatedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
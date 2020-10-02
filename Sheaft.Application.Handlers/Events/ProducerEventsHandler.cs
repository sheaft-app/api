using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class ProducerEventsHandler : EventsHandler,
        INotificationHandler<ProducerNotConfiguredEvent>,
        INotificationHandler<ProducerDocumentsNotCreatedEvent>,
        INotificationHandler<ProducerDocumentsNotReviewedEvent>,
        INotificationHandler<ProducerDocumentsNotValidatedEvent>
    {
        public ProducerEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
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
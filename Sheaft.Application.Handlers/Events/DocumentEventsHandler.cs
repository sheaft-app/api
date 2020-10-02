using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class DocumentEventsHandler : EventsHandler,
        INotificationHandler<DocumentRefusedEvent>,
        INotificationHandler<DocumentOutdatedEvent>,
        INotificationHandler<DocumentValidatedEvent>
    {
        public DocumentEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
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
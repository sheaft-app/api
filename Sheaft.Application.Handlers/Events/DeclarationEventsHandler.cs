using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class DeclarationEventsHandler : EventsHandler,
        INotificationHandler<DeclarationIncompleteEvent>,
        INotificationHandler<DeclarationRefusedEvent>,
        INotificationHandler<DeclarationValidatedEvent>
    {
        public DeclarationEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public Task Handle(DeclarationIncompleteEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(DeclarationRefusedEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(DeclarationValidatedEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
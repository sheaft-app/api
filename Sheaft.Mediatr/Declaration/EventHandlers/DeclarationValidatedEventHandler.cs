using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.Declaration;

namespace Sheaft.Mediatr.Declaration.EventHandlers
{
    public class DeclarationValidatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeclarationValidatedEvent>>
    {
        public DeclarationValidatedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public Task Handle(DomainEventNotification<DeclarationValidatedEvent> notification, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
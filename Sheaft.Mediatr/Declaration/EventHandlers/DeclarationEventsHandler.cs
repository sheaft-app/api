using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;

namespace Sheaft.Mediatr.Declaration.EventHandlers
{
    public class DeclarationEventsHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeclarationIncompleteEvent>>,
        INotificationHandler<DomainEventNotification<DeclarationRefusedEvent>>,
        INotificationHandler<DomainEventNotification<DeclarationValidatedEvent>>
    {
        public DeclarationEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DeclarationIncompleteEvent> notification, CancellationToken token)
        {
            var declarationEvent = notification.DomainEvent;
            var legal = await _context.GetSingleAsync<BusinessLegal>(l => l.Declaration.Id == declarationEvent.DeclarationId, token);
            if (legal.Declaration.Status != DeclarationStatus.Incomplete)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Déclaration UBO du producteur {legal.User.Name} incomplète",
               $"La déclaration d'ubo du producteur {legal.User.Name} ({legal.User.Email}) est incomplète. Raison: {legal.Declaration.ReasonCode}-{legal.Declaration.ReasonMessage}.",
               false,
               token);
        }

        public async Task Handle(DomainEventNotification<DeclarationRefusedEvent> notification, CancellationToken token)
        {
            var declarationEvent = notification.DomainEvent;
            var legal = await _context.GetSingleAsync<BusinessLegal>(l => l.Declaration.Id == declarationEvent.DeclarationId, token);
            if (legal.Declaration.Status != DeclarationStatus.Refused)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Déclaration UBO du producteur {legal.User.Name} refusée",
               $"La déclaration d'ubo du producteur {legal.User.Name} ({legal.User.Email}) a été refusée. Raison: {legal.Declaration.ReasonCode}-{legal.Declaration.ReasonMessage}.",
               false,
               token);
        }

        public Task Handle(DomainEventNotification<DeclarationValidatedEvent> notification, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
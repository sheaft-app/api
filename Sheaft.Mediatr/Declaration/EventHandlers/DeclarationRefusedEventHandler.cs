using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;

namespace Sheaft.Mediatr.Declaration.EventHandlers
{
    public class DeclarationRefusedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeclarationRefusedEvent>>
    {
        public DeclarationRefusedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DeclarationRefusedEvent> notification, CancellationToken token)
        {
            var declarationEvent = notification.DomainEvent;
            var legal = await _context.Set<BusinessLegal>().SingleOrDefaultAsync(l => l.DeclarationId == declarationEvent.DeclarationId, token);
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
    }
}
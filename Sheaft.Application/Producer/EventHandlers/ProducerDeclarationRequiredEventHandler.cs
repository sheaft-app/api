using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Application.Producer.EventHandlers
{
    public class ProducerDeclarationRequiredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProducerDeclarationRequiredEvent>>
    {
        public ProducerDeclarationRequiredEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }
        
        public async Task Handle(DomainEventNotification<ProducerDeclarationRequiredEvent> notification, CancellationToken token)
        {
            var producerEvent = notification.DomainEvent;
            var producer = await _context.GetSingleAsync<Domain.Producer>(c => c.Id == producerEvent.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Un producteur vient de dépasser les 150€/mois de commandes",
               $"Un producteur ({producer.Name}) vient de franchir la limite des 150€/mois sur la plateforme, contactez le par email ({producer.Email}) ou par téléphone ({producer.Phone}) pour valider avec lui les documents banquaires.",
               false,
               token);
        }
    }
}
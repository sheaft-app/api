using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
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
            var producer = await _context.GetSingleAsync<Producer>(c => c.Id == producerEvent.ProducerId, token);
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
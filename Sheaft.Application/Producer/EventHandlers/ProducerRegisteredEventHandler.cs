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
    public class ProducerRegisteredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProducerRegisteredEvent>>
    {
        public ProducerRegisteredEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<ProducerRegisteredEvent> notification, CancellationToken token)
        {
            var producerEvent = notification.DomainEvent;
            var producer = await _context.GetSingleAsync<Producer>(c => c.Id == producerEvent.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Nouveau producteur sur la plateforme",
               $"Un nouveau producteur ({producer.Name}) ({producer.Address.Zipcode}) vient de s'enregistrer sur la plateforme, vous pouvez le contacter par email ({producer.Email}) ou par téléphone ({producer.Phone}).",
               false,
               token);
        }
    }
}
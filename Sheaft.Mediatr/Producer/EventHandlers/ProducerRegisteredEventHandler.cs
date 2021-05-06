using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Mediatr.Producer.EventHandlers
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
            var producer = await _context.Producers.SingleAsync(e => e.Id == producerEvent.ProducerId, token);
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
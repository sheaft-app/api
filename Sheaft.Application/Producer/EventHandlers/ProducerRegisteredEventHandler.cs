using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Application.Producer.EventHandlers
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
            var producer = await _context.GetSingleAsync<Domain.Producer>(c => c.Id == producerEvent.ProducerId, token);
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
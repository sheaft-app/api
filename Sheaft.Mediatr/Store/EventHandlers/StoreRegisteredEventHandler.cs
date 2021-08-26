using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.Store;

namespace Sheaft.Mediatr.Store.EventHandlers
{
    public class StoreRegisteredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<StoreRegisteredEvent>>
    {
        public StoreRegisteredEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<StoreRegisteredEvent> notification, CancellationToken token)
        {
            var storeEvent = notification.DomainEvent;
            var store = await _context.Stores.SingleAsync(e => e.Id == storeEvent.StoreId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Nouveau magasin sur la plateforme",
               $"Un nouveau magasin ({store.Name}) ({store.Address.Zipcode}) vient de s'enregistrer sur la plateforme, vous pouvez le contacter par email ({store.Email}) ou par téléphone ({store.Phone}).",
               false,
               token);
        }
    }
}
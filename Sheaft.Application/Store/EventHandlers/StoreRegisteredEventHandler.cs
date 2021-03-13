using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Store;

namespace Sheaft.Application.Store.EventHandlers
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
            var store = await _context.GetSingleAsync<Domain.Store>(c => c.Id == storeEvent.StoreId, token);
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
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class StoreEventsHandler : EventsHandler,
        INotificationHandler<StoreRegisteredEvent>
    {
        public StoreEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(StoreRegisteredEvent notification, CancellationToken token)
        {
            var store = await _context.GetSingleAsync<Store>(c => c.Id == notification.StoreId, token);
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
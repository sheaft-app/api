using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class OrderEventsHandler : EventsHandler,
        INotificationHandler<ConfirmOrderFailedEvent>
    {
        public OrderEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public async Task Handle(ConfirmOrderFailedEvent notification, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Order>(notification.OrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La confirmation de l'order {order.Reference} a échoué",
               $"La confirmation de l'order {order.Reference} d'un montant de {order.TotalPrice}€ pour {order.User.Name} ({order.User.Email}) a échoué. Raison: {notification.Message}.",
               false,
               token);
        }
    }
}
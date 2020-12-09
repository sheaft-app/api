using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class WithholdingEventsHandler : EventsHandler,
        INotificationHandler<WithholdingFailedEvent>
    {
        public WithholdingEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(WithholdingFailedEvent notification, CancellationToken token)
        {
            var withholding = await _context.GetByIdAsync<Withholding>(notification.WithholdingId, token);
            if (withholding.Status != TransactionStatus.Failed)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La retenue de {withholding.Debited}€ {withholding.Author.Name} a échouée",
               $"La retenue de {withholding.Debited}€ pour le producteur {withholding.Author.Name} ({withholding.Author.Email}) a échouée. Raison: {withholding.ResultCode}-{withholding.ResultMessage}.",
               false,
               token);
        }

        public async Task Handle(CreateDonationFailedEvent notification, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Order>(notification.OrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La création du transfert pour la donation de {order.Donate - order.InternalFeesPrice}€ de {order.User.Name} a échouée",
               $"La création du transfert pour la donation de {order.Donate - order.InternalFeesPrice}€ de {order.User.Name} a échouée à plus de 3 reprises, la donation est mise en supsend, veuillez traiter manuellement la demande.",
               false,
               token);
        }
    }
}
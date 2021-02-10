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
    public class WithholdingFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<WithholdingFailedEvent>>
    {
        public WithholdingFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<WithholdingFailedEvent> notification, CancellationToken token)
        {
            var withholdingEvent = notification.DomainEvent;
            var withholding = await _context.GetByIdAsync<Withholding>(withholdingEvent.WithholdingId, token);
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
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Withholding;

namespace Sheaft.Mediatr.Withholding.EventHandlers
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
            var withholding = await _context.Withholdings.SingleAsync(e => e.Id == withholdingEvent.WithholdingId, token);
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
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class PayoutRefundEventsHandler : EventsHandler,
        INotificationHandler<PayoutRefundFailedEvent>
    {
        public PayoutRefundEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public async Task Handle(PayoutRefundFailedEvent notification, CancellationToken token)
        {
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Le remboursement au producteur du virement {notification.RefundIdentifier}€ a échoué",
               $"Le remboursement au producteur du virement {notification.RefundIdentifier}€ a échoué.",
               false,
               token);
        }
    }
}
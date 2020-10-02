using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class PayinEventsHandler : EventsHandler,
        INotificationHandler<PayinFailedEvent>,
        INotificationHandler<PayinSucceededEvent>
    {
        public PayinEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public Task Handle(PayinFailedEvent payinEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PayinSucceededEvent payinEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
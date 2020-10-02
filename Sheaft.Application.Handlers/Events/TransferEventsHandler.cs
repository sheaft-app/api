using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class TransferEventsHandler : EventsHandler,
    INotificationHandler<TransferFailedEvent>,
    INotificationHandler<TransferSucceededEvent>
    {
        public TransferEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public Task Handle(TransferFailedEvent transferEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TransferSucceededEvent transferEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
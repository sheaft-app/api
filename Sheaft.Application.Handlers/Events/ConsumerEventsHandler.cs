using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class ConsumerEventsHandler : EventsHandler
    {
        public ConsumerEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }
    }
}
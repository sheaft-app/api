using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class ConsumerEventsHandler :
        INotificationHandler<ConsumerNotConfiguredEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public ConsumerEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(ConsumerNotConfiguredEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
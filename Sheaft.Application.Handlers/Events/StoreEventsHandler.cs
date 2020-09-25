using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class StoreEventsHandler :
        INotificationHandler<StoreNotConfiguredEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public StoreEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(StoreNotConfiguredEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
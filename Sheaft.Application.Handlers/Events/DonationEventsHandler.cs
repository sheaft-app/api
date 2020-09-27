using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Handlers
{
    public class DonationEventsHandler :
        INotificationHandler<DonationFailedEvent>,
        INotificationHandler<DonationSucceededEvent>,
        INotificationHandler<CreateDonationFailedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public DonationEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(DonationFailedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(DonationSucceededEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(CreateDonationFailedEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
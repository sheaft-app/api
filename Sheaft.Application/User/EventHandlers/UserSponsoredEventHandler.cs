using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Application.Models.Mailer;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class UserSponsoredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserSponsoredEvent>>
    {
        private readonly IConfiguration _configuration;

        public UserSponsoredEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<UserSponsoredEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var user = await _context.GetByIdAsync<User>(userEvent.SponsorId, token);
            var sponsoring = await _context.GetSingleAsync<Sponsoring>(c => c.Sponsor.Id == userEvent.SponsorId && c.Sponsored.Id == userEvent.SponsoredId, token);

            await _signalrService.SendNotificationToUserAsync(userEvent.SponsorId, "SponsoringUsedEvent", new { SponsoredName = sponsoring.Sponsored.Name });
            await _signalrService.SendNotificationToUserAsync(userEvent.SponsoredId, "NewSponsoredEvent", new { SponsorName = user.Name });
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.User;

namespace Sheaft.Application.User.EventHandlers
{
    public class UserSponsoredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserSponsoredEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly ISheaftMediatr _mediatr;

        public UserSponsoredEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            ISheaftMediatr mediatr)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _mediatr = mediatr;
        }

        public async Task Handle(DomainEventNotification<UserSponsoredEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var user = await _context.GetByIdAsync<Domain.User>(userEvent.SponsorId, token);
            var sponsoring = await _context.GetSingleAsync<Sponsoring>(c => c.Sponsor.Id == userEvent.SponsorId && c.Sponsored.Id == userEvent.SponsoredId, token);

            await _signalrService.SendNotificationToUserAsync(userEvent.SponsorId, "SponsoringUsedEvent", new { SponsoredName = sponsoring.Sponsored.Name });
            await _signalrService.SendNotificationToUserAsync(userEvent.SponsoredId, "NewSponsoredEvent", new { SponsorName = user.Name });
            
            _mediatr.Post(new CreateUserPointsCommand(new RequestUser())
            {
                CreatedOn = userEvent.CreatedOn,
                Kind = PointKind.Sponsoring,
                UserId = userEvent.SponsorId
            });

        }
    }
}
﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Events.User;

namespace Sheaft.Mediatr.User.EventHandlers
{
    public class UserSponsoredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserSponsoredEvent>>
    {
        public UserSponsoredEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<UserSponsoredEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var user = await _context.GetByIdAsync<Domain.User>(userEvent.SponsorId, token);
            var sponsoring = await _context.GetSingleAsync<Sponsoring>(
                c => c.Sponsor.Id == userEvent.SponsorId && c.Sponsored.Id == userEvent.SponsoredId, token);

            await _signalrService.SendNotificationToUserAsync(userEvent.SponsorId, "SponsoringUsedEvent",
                new {SponsoredName = sponsoring.Sponsored.Name});
            await _signalrService.SendNotificationToUserAsync(userEvent.SponsoredId, "NewSponsoredEvent",
                new {SponsorName = user.Name});
        }
    }
}
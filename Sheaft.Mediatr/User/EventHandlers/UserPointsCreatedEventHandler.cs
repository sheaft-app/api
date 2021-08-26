using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.User;

namespace Sheaft.Mediatr.User.EventHandlers
{
    public class UserPointsCreatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserPointsCreatedEvent>>
    {
        public UserPointsCreatedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<UserPointsCreatedEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            await _signalrService.SendNotificationToUserAsync(userEvent.UserId, nameof(UserPointsCreatedEvent),
                userEvent);
        }
    }
}
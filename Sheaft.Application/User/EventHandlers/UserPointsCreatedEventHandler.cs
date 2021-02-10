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
    public class UserPointsCreatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserPointsCreatedEvent>>
    {
        private readonly IConfiguration _configuration;

        public UserPointsCreatedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<UserPointsCreatedEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            await _signalrService.SendNotificationToUserAsync(userEvent.UserId, nameof(UserPointsCreatedEvent), userEvent);
        }
    }
}
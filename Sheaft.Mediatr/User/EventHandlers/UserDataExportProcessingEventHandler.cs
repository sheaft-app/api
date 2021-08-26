using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.User;

namespace Sheaft.Mediatr.User.EventHandlers
{
    public class UserDataExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportProcessingEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public UserDataExportProcessingEventHandler(
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<UserDataExportProcessingEvent> notification,
            CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == userEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(UserDataExportProcessingEvent),
                new {JobId = jobIdentifier, UserId = job.UserId});
        }
    }
}
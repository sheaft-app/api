using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.User;

namespace Sheaft.Mediatr.User.EventHandlers
{
    public class UserDataExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportProcessingEvent>>
    {
        public UserDataExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<UserDataExportProcessingEvent> notification,
            CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.User.Id, nameof(UserDataExportProcessingEvent),
                new {JobId = userEvent.JobId, UserId = job.User.Id});
        }
    }
}
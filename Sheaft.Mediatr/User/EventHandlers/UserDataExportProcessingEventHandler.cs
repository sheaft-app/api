using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
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
            var job = await _context.Jobs.SingleAsync(e => e.Id == userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(UserDataExportProcessingEvent),
                new {JobId = userEvent.JobId, UserId = job.UserId});
        }
    }
}
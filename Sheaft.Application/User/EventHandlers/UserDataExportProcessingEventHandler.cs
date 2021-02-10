using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.User;

namespace Sheaft.Application.User.EventHandlers
{
    public class UserDataExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportProcessingEvent>>
    {
        private readonly IConfiguration _configuration;

        public UserDataExportProcessingEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }
        
        public async Task Handle(DomainEventNotification<UserDataExportProcessingEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.User.Id, nameof(UserDataExportProcessingEvent), new { JobId = userEvent.JobId, UserId = job.User.Id });
        }
    }
}
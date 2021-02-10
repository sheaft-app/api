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
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(UserDataExportProcessingEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id });
        }
    }
}
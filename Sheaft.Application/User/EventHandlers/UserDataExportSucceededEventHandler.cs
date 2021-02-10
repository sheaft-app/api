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
    public class UserDataExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportSucceededEvent>>
    {
        private readonly IConfiguration _configuration;

        public UserDataExportSucceededEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }
        
        public async Task Handle(DomainEventNotification<UserDataExportSucceededEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);

            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(UserDataExportSucceededEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id, Url = job.File });
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de données est prêt",
                nameof(UserDataExportSucceededEvent),
                new RgpdExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File },
                true,
                token);            
        }
    }
}
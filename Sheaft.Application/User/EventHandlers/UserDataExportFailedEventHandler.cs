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
    public class UserDataExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;

        public UserDataExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<UserDataExportFailedEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);

            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(UserDataExportFailedEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/account/profile";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de données a échoué",
                nameof(UserDataExportFailedEvent),
                new RgpdExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, PortalUrl = url },
                true,
                token);
        }
    }
}
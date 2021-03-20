using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Events.User;

namespace Sheaft.Mediatr.User.EventHandlers
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
            var job = await _context.GetByIdAsync<Domain.Job>(userEvent.JobId, token);

            await _signalrService.SendNotificationToUserAsync(job.User.Id, nameof(UserDataExportFailedEvent), new { JobId = userEvent.JobId, UserId = job.User.Id });

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
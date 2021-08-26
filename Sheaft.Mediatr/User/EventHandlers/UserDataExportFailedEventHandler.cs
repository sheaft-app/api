using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.User;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.User.EventHandlers
{
    public class UserDataExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public UserDataExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<UserDataExportFailedEvent> notification, CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == userEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);

            await _signalrService.SendNotificationToUserAsync(job.User.Id, nameof(UserDataExportFailedEvent), new { JobId = jobIdentifier, UserId = job.UserId });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{jobIdentifier}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de données a échoué",
                nameof(UserDataExportFailedEvent),
                new RgpdExportMailerModel { JobId = jobIdentifier, UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, PortalUrl = url },
                true,
                token);
        }
    }
}
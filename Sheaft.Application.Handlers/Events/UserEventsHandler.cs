using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models.Mailer;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class UserEventsHandler : EventsHandler,
        INotificationHandler<UserDataExportFailedEvent>,
        INotificationHandler<UserDataExportSucceededEvent>,
        INotificationHandler<UserDataExportProcessingEvent>,
        INotificationHandler<UserSponsoredEvent>,
        INotificationHandler<UserPointsCreatedEvent>
    {
        private readonly IConfiguration _configuration;

        public UserEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(UserDataExportFailedEvent userEvent, CancellationToken token)
        {
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

        public async Task Handle(UserDataExportSucceededEvent userEvent, CancellationToken token)
        {
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

        public async Task Handle(UserDataExportProcessingEvent userEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(UserDataExportProcessingEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id });
        }

        public async Task Handle(UserSponsoredEvent userEvent, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<User>(userEvent.SponsorId, token);
            var sponsoring = await _context.GetSingleAsync<Sponsoring>(c => c.Sponsor.Id == userEvent.SponsorId && c.Sponsored.Id == userEvent.SponsoredId, token);

            await _signalrService.SendNotificationToUserAsync(userEvent.SponsorId, nameof(SponsoringUsedEvent), new { SponsoredName = sponsoring.Sponsored.Name });
            await _signalrService.SendNotificationToUserAsync(userEvent.SponsoredId, nameof(NewSponsoredEvent), new { SponsorName = user.Name });
        }

        public async Task Handle(UserPointsCreatedEvent userEvent, CancellationToken token)
        {
            await _signalrService.SendNotificationToUserAsync(userEvent.UserId, nameof(UserPointsCreatedEvent), userEvent);
        }
    }
}
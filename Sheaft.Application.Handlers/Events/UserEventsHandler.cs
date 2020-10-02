using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
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
        public UserEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public async Task Handle(UserDataExportFailedEvent userEvent, CancellationToken token)
        {
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(UserDataExportFailedEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id });
        }

        public async Task Handle(UserDataExportSucceededEvent userEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(UserDataExportSucceededEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id, Url = job.File });

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                _emailTemplateOptions.UserDataExportSucceededEvent,
                new { UserName = job.User.Name, job.CreatedOn, DownloadUrl = job.File },
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
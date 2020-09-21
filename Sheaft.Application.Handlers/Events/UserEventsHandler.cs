using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class UserEventsHandler :
        INotificationHandler<ExportUserDataFailedEvent>,
        INotificationHandler<ExportUserDataSucceededEvent>,
        INotificationHandler<ExportUserDataProcessingEvent>,
        INotificationHandler<UserSponsoredEvent>,
        INotificationHandler<UserPointsCreatedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISignalrService _signalrService;

        public UserEventsHandler(IAppDbContext context, IEmailService emailService, ISignalrService signalrService)
        {
            _context = context;
            _emailService = emailService;
            _signalrService = signalrService;
        }

        public async Task Handle(ExportUserDataFailedEvent userEvent, CancellationToken token)
        {
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(ExportUserDataFailedEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id });
        }

        public async Task Handle(ExportUserDataSucceededEvent userEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(ExportUserDataSucceededEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id, Url = job.File });

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                ExportUserDataSucceededEvent.MAILING_TEMPLATE_ID,
                new { UserName = job.User.Name, job.CreatedOn, DownloadUrl = job.File },
                token);            
        }

        public async Task Handle(ExportUserDataProcessingEvent userEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(userEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(userEvent.RequestUser.Id, nameof(ExportUserDataProcessingEvent), new { JobId = userEvent.JobId, UserId = userEvent.RequestUser.Id });
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
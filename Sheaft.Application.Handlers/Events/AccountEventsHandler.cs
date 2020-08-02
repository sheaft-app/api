using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class AccountEventsHandler :
        INotificationHandler<AccountExportDataFailedEvent>,
        INotificationHandler<AccountExportDataSucceededEvent>,
        INotificationHandler<AccountExportDataProcessingEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISignalrService _signalrService;

        public AccountEventsHandler(IAppDbContext context, IEmailService emailService, ISignalrService signalrService)
        {
            _context = context;
            _emailService = emailService;
            _signalrService = signalrService;
        }

        public async Task Handle(AccountExportDataFailedEvent appEvent, CancellationToken token)
        {
            await _signalrService.SendNotificationToUserAsync(appEvent.RequestUser.Id, nameof(AccountExportDataFailedEvent), new { JobId = appEvent.JobId, UserId = appEvent.RequestUser.Id });
        }

        public async Task Handle(AccountExportDataSucceededEvent appEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(appEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(appEvent.RequestUser.Id, nameof(AccountExportDataSucceededEvent), new { JobId = appEvent.JobId, UserId = appEvent.RequestUser.Id, Url = job.File });

            var userName = $"{job.User.FirstName} {job.User.LastName}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                userName,
                AccountExportDataSucceededEvent.MAILING_TEMPLATE_ID,
                new { UserName = userName, job.CreatedOn, DownloadUrl = job.File },
                token);            
        }

        public async Task Handle(AccountExportDataProcessingEvent appEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(appEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(appEvent.RequestUser.Id, nameof(AccountExportDataProcessingEvent), new { JobId = appEvent.JobId, UserId = appEvent.RequestUser.Id });
        }
    }
}
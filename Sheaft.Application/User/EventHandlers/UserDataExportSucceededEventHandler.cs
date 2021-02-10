using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Mailer;
using Sheaft.Domain.Events.User;

namespace Sheaft.Application.User.EventHandlers
{
    public class UserDataExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportSucceededEvent>>
    {
        public UserDataExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<UserDataExportSucceededEvent> notification,
            CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(userEvent.JobId, token);

            await _signalrService.SendNotificationToUserAsync(job.User.Id, nameof(UserDataExportSucceededEvent),
                new {JobId = userEvent.JobId, UserId = job.User.Id, Url = job.File});
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de données est prêt",
                nameof(UserDataExportSucceededEvent),
                new RgpdExportMailerModel
                    {UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}
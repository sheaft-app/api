using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.User;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.User.EventHandlers
{
    public class UserDataExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<UserDataExportSucceededEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public UserDataExportSucceededEventHandler(
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<UserDataExportSucceededEvent> notification,
            CancellationToken token)
        {
            var userEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == userEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);

            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(UserDataExportSucceededEvent),
                new {JobId = jobIdentifier, UserId = job.UserId, Url = job.File});
            
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de données est prêt",
                nameof(UserDataExportSucceededEvent),
                new RgpdExportMailerModel
                    {JobId = jobIdentifier,UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}
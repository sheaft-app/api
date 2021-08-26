using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Billing.EventHandlers
{
    public class BillingExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<BillingExportSucceededEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public BillingExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<BillingExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(BillingExportSucceededEvent),
                new {JobId = jobIdentifier, Name = job.Name, UserId = job.UserId, Url = job.File});

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de facturation est prêt",
                nameof(BillingExportSucceededEvent),
                new BillingExportMailerModel
                    {UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}
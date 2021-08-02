using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PickingOrder;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.PickingOrder.EventHandlers
{
    public class PickingOrderExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportSucceededEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public PickingOrderExportSucceededEventHandler(
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<PickingOrderExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(PickingOrderExportSucceededEvent),
                new {JobId = jobIdentifier, Name = job.Name, UserId = job.UserId, Url = job.File});

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre bon de préparation est prêt",
                nameof(PickingOrderExportSucceededEvent),
                new PickingOrderExportMailerModel
                    {JobId = jobIdentifier, UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.PickingOrder;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.PickingOrder.EventHandlers
{
    public class PickingOrderExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public PickingOrderExportFailedEventHandler(
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

        public async Task Handle(DomainEventNotification<PickingOrderExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            await _signalrService.SendNotificationToUserAsync(job.User.Id, nameof(PickingOrderExportFailedEvent), new { JobId = jobIdentifier, Name = job.Name, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{jobIdentifier}?refresh={Guid.NewGuid():N}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La création de votre bon de préparation a échouée",
                nameof(PickingOrderExportFailedEvent),
                new PickingOrderExportMailerModel { JobId = jobIdentifier, UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}
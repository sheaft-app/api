using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrdersExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrdersExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrdersExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrdersExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(PurchaseOrdersExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.UserId });

            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{jobIdentifier}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La création de votre export de commandes a échouée",
                nameof(PurchaseOrdersExportFailedEvent),
                new PurchaseOrdersExportMailerModel { JobId = jobIdentifier, UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}
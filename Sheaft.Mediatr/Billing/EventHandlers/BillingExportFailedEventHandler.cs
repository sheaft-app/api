using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Billing.EventHandlers
{
    public class BillingExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<BillingExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public BillingExportFailedEventHandler(
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

        public async Task Handle(DomainEventNotification<BillingExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(BillingExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.UserId });

            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{jobIdentifier}?refresh={Guid.NewGuid():N}";
            
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La création de votre export de facturation a échouée",
                nameof(BillingExportFailedEvent),
                new BillingExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}
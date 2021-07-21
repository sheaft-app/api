using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Billing.EventHandlers
{
    public class BillingExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<BillingExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;

        public BillingExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<BillingExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(BillingExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.UserId });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{job.Id:N}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La création de votre export de livraisons a échouée",
                nameof(BillingExportFailedEvent),
                new BillingExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Handlers
{
    public class ProductImportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProductImportProcessingEvent>>
    {
        private readonly IConfiguration _configuration;

        public ProductImportProcessingEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<ProductImportProcessingEvent> notification, CancellationToken token)
        {
            var productEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportProcessingEvent), new { JobId = job.Id, UserId = job.User.Id });
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.DeliveryBatch;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.DeliveryBatch.EventHandlers
{
    public class DeliveryBatchFormsGeneratedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryBatchFormsGeneratedEvent>>
    {
        private readonly IBlobService _blobService;

        public DeliveryBatchFormsGeneratedEventHandler(
            IAppDbContext context,
            IBlobService blobService,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _blobService = blobService;
        }

        public async Task Handle(DomainEventNotification<DeliveryBatchFormsGeneratedEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var deliveryBatch = await _context.DeliveryBatches.SingleAsync(d => d.Id == @event.DeliveryBatchId, token);
            if (string.IsNullOrWhiteSpace(deliveryBatch.DeliveryFormsUrl))
                return;

            var templateId = deliveryBatch.Status == DeliveryBatchStatus.Completed
                ? nameof(DeliveryBatchFormsCompletedEvent)
                : nameof(DeliveryBatchFormsGeneratedEvent);
            
            await _signalrService.SendNotificationToUserAsync(deliveryBatch.AssignedToId, templateId,
                new {Url = deliveryBatch.DeliveryFormsUrl});

            var fileResult = await _blobService.DownloadDeliveryBatchFormsAsync(deliveryBatch.DeliveryFormsUrl, token);
            if (!fileResult.Succeeded)
                throw fileResult.Exception;

            var subject = deliveryBatch.Status == DeliveryBatchStatus.Completed
                ? $"Compte rendu {deliveryBatch.Name} du {deliveryBatch.ScheduledOn:dd/MM/yyyy}"
                : $"{deliveryBatch.Name} - Bons de livraisons du {deliveryBatch.ScheduledOn:dd/MM/yyyy}"; 
            
            await _emailService.SendTemplatedEmailAsync(
                deliveryBatch.AssignedTo.Email,
                deliveryBatch.AssignedTo.Name,
                subject,
                templateId,
                new DeliveryBatchMailerModel
                {
                    Firstname = deliveryBatch.AssignedTo.FirstName,
                    Name = deliveryBatch.Name,
                    Url = deliveryBatch.DeliveryFormsUrl, 
                    ScheduledOn = deliveryBatch.ScheduledOn
                },
                new List<EmailAttachmentDto>
                {
                    new EmailAttachmentDto
                    {
                        Content = fileResult.Data,
                        Name = $"{(deliveryBatch.Status == DeliveryBatchStatus.Completed ? "CR - " : "")}{deliveryBatch.Name}_{deliveryBatch.ScheduledOn:yyyyMMdd}.pdf"
                    }
                },
                true,
                token);
        }
    }
}
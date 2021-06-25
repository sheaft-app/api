using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Extensions;
using Sheaft.Mailing;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.Mediatr.Delivery.EventHandlers
{
    public class DeliveryReceiptGeneratedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryReceiptGeneratedEvent>>
    {
        private readonly ISheaftMediatr _mediatr;
        private readonly IBlobService _blobService;

        public DeliveryReceiptGeneratedEventHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _mediatr = mediatr;
            _blobService = blobService;
        }

        public async Task Handle(DomainEventNotification<DeliveryReceiptGeneratedEvent> notification,
            CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var delivery = await _context.Deliveries.SingleAsync(d => d.Id == @event.DeliveryId, token);
            if (delivery.Status != DeliveryStatus.Ready || delivery.DeliveryBatch is {Status: DeliveryBatchStatus.Cancelled})
                return;

            await _signalrService.SendNotificationToUserAsync(delivery.ClientId, nameof(DeliveryReceiptGeneratedEvent),
                new {Url = delivery.DeliveryFormUrl});

            var client = await _context.Users.SingleAsync(u => u.Id == delivery.ClientId, token);
            var producer = await _context.Producers.SingleAsync(u => u.Id == delivery.ProducerId, token);

            var blobResult = await _blobService.DownloadDeliveryAsync(delivery.DeliveryReceiptUrl, token);
            if (!blobResult.Succeeded)
                throw SheaftException.BadRequest(blobResult.Exception);

            await _emailService.SendTemplatedEmailAsync(
                client.Email,
                client.Name,
                $"{producer.Name} - {delivery.Reference.AsReceiptIdentifier()} du {delivery.ScheduledOn:dd/MM/yyyy}",
                nameof(DeliveryReceiptGeneratedEvent),
                new DeliveryMailerModel
                {
                    Firstname = client.FirstName,
                    Url = delivery.DeliveryReceiptUrl, DeliveredOn = delivery.DeliveredOn,
                    ProducerName = producer.Name, ScheduledOn = delivery.ScheduledOn
                },
                new List<EmailAttachmentDto>
                {
                    new EmailAttachmentDto()
                    {
                        Content = blobResult.Data,
                        Name =  $"{producer.Name.Replace(" ", "")}_{delivery.Reference.AsReceiptIdentifier()}_{delivery.ScheduledOn:dd/MM/yyyy}",
                    }
                },
                true,
                token);
        }
    }
}
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
    public class DeliveryFormGeneratedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryFormGeneratedEvent>>
    {
        private readonly ISheaftMediatr _mediatr;
        private readonly IBlobService _blobService;

        public DeliveryFormGeneratedEventHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IEmailService emailService,
            IBlobService blobService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _mediatr = mediatr;
            _blobService = blobService;
        }

        public async Task Handle(DomainEventNotification<DeliveryFormGeneratedEvent> notification,
            CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var delivery = await _context.Deliveries.SingleAsync(d => d.Id == @event.DeliveryId, token);
            if (delivery.Status != DeliveryStatus.Delivered || delivery.DeliveryBatch is {Status: DeliveryBatchStatus.Cancelled})
                return;

            var client = await _context.Users.SingleAsync(u => u.Id == delivery.ClientId, token);
            if (client.Kind != ProfileKind.Store)
                return;
            
            var producer = await _context.Producers.SingleAsync(u => u.Id == delivery.ProducerId, token);

            var blobResult = await _blobService.DownloadDeliveryAsync(delivery.DeliveryFormUrl, token);
            if (!blobResult.Succeeded)
                throw SheaftException.BadRequest(blobResult.Exception);
            
            await _signalrService.SendNotificationToUserAsync(delivery.ClientId, nameof(DeliveryFormGeneratedEvent),
                new {Url = delivery.DeliveryFormUrl});
            
            await _emailService.SendTemplatedEmailAsync(
                client.Email,
                client.Name,
                $"{producer.Name} - {delivery.Reference.AsDeliveryIdentifier()} du {delivery.DeliveredOn.Value:dd/MM/yyyy}",
                nameof(DeliveryFormGeneratedEvent),
                new DeliveryMailerModel
                {
                    Firstname = client.FirstName,
                    Url = delivery.DeliveryFormUrl, DeliveredOn = delivery.DeliveredOn,
                    ProducerName = producer.Name, ScheduledOn = delivery.ScheduledOn
                },
                new List<EmailAttachmentDto>
                {
                    new EmailAttachmentDto()
                    {
                        Content = blobResult.Data,
                        Name =  $"{delivery.Reference.AsDeliveryIdentifier()}.pdf",
                    }
                },
                true,
                token);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Extensions;
using Sheaft.Mailing;

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
            if (delivery.ReceiptSentOn.HasValue || delivery.Status != DeliveryStatus.Ready || delivery.DeliveryBatch is {Status: DeliveryBatchStatus.Cancelled})
                return;

            var client = await _context.Users.SingleAsync(u => u.Id == delivery.ClientId, token);
            var producer = await _context.Producers.SingleAsync(u => u.Id == delivery.ProducerId, token);

            var blobResult = await _blobService.DownloadDeliveryAsync(delivery.DeliveryReceiptUrl, token);
            if (!blobResult.Succeeded)
                throw SheaftException.BadRequest(blobResult.Exception);

            await _signalrService.SendNotificationToUserAsync(delivery.ClientId, nameof(DeliveryReceiptGeneratedEvent),
                new {Url = delivery.DeliveryReceiptUrl});

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
                        Name =  $"{delivery.Reference.AsReceiptIdentifier()}.pdf",
                    }
                },
                true,
                token);

            delivery.ReceiptSentOn = DateTimeOffset.UtcNow;
        }
    }
}
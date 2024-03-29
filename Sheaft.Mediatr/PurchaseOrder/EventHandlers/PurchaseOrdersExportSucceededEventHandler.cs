﻿using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrdersExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrdersExportSucceededEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrdersExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrdersExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(PurchaseOrdersExportSucceededEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.UserId, Url = job.File});

            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de commandes est prêt",
                nameof(PurchaseOrdersExportSucceededEvent),
                new PurchaseOrdersExportMailerModel
                    {JobId = jobIdentifier, UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}
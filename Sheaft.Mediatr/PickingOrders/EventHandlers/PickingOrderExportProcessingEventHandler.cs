﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PickingOrder;

namespace Sheaft.Mediatr.PickingOrders.EventHandlers
{
    public class PickingOrderExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportProcessingEvent>>
    {
        public PickingOrderExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PickingOrderExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportProcessingEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.User.Id});
        }
    }
}
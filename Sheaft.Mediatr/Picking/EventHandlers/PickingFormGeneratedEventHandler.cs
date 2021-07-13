using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Picking;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Picking.EventHandlers
{
    public class PickingFormGeneratedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingFormGeneratedEvent>>
    {
        private readonly ISheaftMediatr _mediatr;
        private readonly IBlobService _blobService;

        public PickingFormGeneratedEventHandler(
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

        public async Task Handle(DomainEventNotification<PickingFormGeneratedEvent> notification,
            CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var picking = await _context.Pickings.SingleAsync(d => d.Id == @event.PickingId, token);
            if (picking.Status == PickingStatus.Completed)
                return;

            var producer = await _context.Producers.SingleAsync(u => u.Id == picking.ProducerId, token);
            
            var blobResult = await _blobService.DownloadDeliveryAsync(picking.PickingFormUrl, token);
            if (!blobResult.Succeeded)
                throw SheaftException.BadRequest(blobResult.Exception);
            
            await _signalrService.SendNotificationToUserAsync(picking.ProducerId, nameof(PickingFormGeneratedEvent),
                new {Url = picking.PickingFormUrl});
            
            await _emailService.SendTemplatedEmailAsync(
                producer.Email,
                producer.Name,
                picking.Name,
                nameof(PickingFormGeneratedEvent),
                new PickingMailerModel
                {
                    UserName = producer.FirstName,
                    DownloadUrl = picking.PickingFormUrl,
                    Name = producer.Name,
                },
                new List<EmailAttachmentDto>
                {
                    new EmailAttachmentDto()
                    {
                        Content = blobResult.Data,
                        Name =  $"Preparation_{picking.CreatedOn:dd/MM/yyyy}.xlsx",
                    }
                },
                true,
                token);
        }
    }
}
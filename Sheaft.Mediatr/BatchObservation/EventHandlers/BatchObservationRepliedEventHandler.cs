using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.BatchObservation;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.BatchObservation.EventHandlers
{
    public class BatchObservationRepliedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<BatchObservationRepliedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public BatchObservationRepliedEventHandler(
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

        public async Task Handle(DomainEventNotification<BatchObservationRepliedEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var batchObservation = await _context.Set<Domain.BatchObservation>().SingleAsync(e => e.Id == @event.BatchObservationId, token);
            var batchReply = await _context.Set<Domain.BatchObservation>().SingleAsync(e => e.Id == @event.ReplyId, token);
            var batch = await _context.Batches.SingleAsync(e => e.Id == batchReply.BatchId, token);

            var targets = new List<Tuple<Guid, string, string>>();
            if (batchReply.User.Kind != ProfileKind.Producer)
                targets.Add(new Tuple<Guid, string, string>(batch.Producer.Id, batch.Producer.Name, batch.Producer.Email));
            else
            {
                targets.Add(new Tuple<Guid, string, string>(batchObservation.User.Id, batchObservation.User.Name, batchObservation.User.Email));
                targets.AddRange(batchObservation.Replies.Where(r => r.UserId != batch.ProducerId).Select(r =>
                    new Tuple<Guid, string, string>(r.User.Id, r.User.Name, r.User.Email)));
                
                targets = targets.Distinct().ToList();
            }

            foreach (var target in targets)
            {
                await _signalrService.SendNotificationToGroupAsync(target.Item1, nameof(BatchObservationRepliedEvent), batch.GetObservationNotificationContent(
                    _idSerializer.Serialize("Query", nameof(Batch), batch.Id), 
                    _idSerializer.Serialize("Query", nameof(BatchObservation), batchObservation.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    batchReply.User.Name));
            
                await _emailService.SendTemplatedEmailAsync(
                    target.Item3,
                    target.Item2,
                    $"{batchReply.User.Name} a répondu à votre remarque concernant le lot {batch.Number}",
                    nameof(BatchObservationRepliedEvent),
                    batch.GetObservationNotificationData(
                        _idSerializer.Serialize("Query", nameof(Batch), batch.Id), 
                        _idSerializer.Serialize("Query", nameof(BatchObservation), batchObservation.Id),
                        _configuration.GetValue<string>("Portal:url"),
                        batchReply.Comment,
                        batchReply.User.Name),
                    true,
                    token);
            }
        }
    }
}
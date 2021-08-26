using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain;
using Sheaft.Domain.Events.Recall;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Recall.EventHandlers
{
    public class RecallSentEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<RecallSentEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public RecallSentEventHandler(
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

        public async Task Handle(DomainEventNotification<RecallSentEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var recall = await _context.Recalls.SingleAsync(e => e.Id == @event.RecallId, token);

            var clientRecall = await _context.Set<RecallClient>()
                .SingleAsync(c => c.ClientId == @event.ClientId && c.RecallId == @event.RecallId, token);

            if (clientRecall.RecallSent)
                return;
            
            await _signalrService.SendNotificationToUserAsync(clientRecall.Client.Id, nameof(RecallSentEvent),
                recall.GetNotificationContent(
                    _idSerializer.Serialize("Query", nameof(Domain.Recall), recall.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    recall.Producer.Name));
            
            try
            {
                await _emailService.SendTemplatedEmailAsync(
                    clientRecall.Client.Email,
                    clientRecall.Client.Name,
                    $"{recall.Producer.Name} procède au rappel de certains produits.",
                    nameof(RecallSentEvent),
                    recall.GetNotificationData(
                        _idSerializer.Serialize("Query", nameof(Domain.Recall), recall.Id),
                        _configuration.GetValue<string>("Portal:url"),
                        recall.Comment,
                        clientRecall.Client.FirstName),
                    true,
                    token);
                
                clientRecall.SetRecallAsSent();
                await _context.SaveChangesAsync(token);
            }
            catch (Exception e)
            {
                clientRecall.SetRecallAsSent(false);
                await _context.SaveChangesAsync(token);
            }
        }
    }
}
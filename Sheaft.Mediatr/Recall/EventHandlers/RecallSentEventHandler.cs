using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Observation;
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

            var client = await _context.Users
                .SingleAsync(c => c.Id == @event.ClientId, token);

            await _signalrService.SendNotificationToGroupAsync(client.Id, nameof(RecallSentEvent),
                recall.GetNotificationContent(
                    _idSerializer.Serialize("Query", nameof(Domain.Recall), recall.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    recall.Producer.Name));

            await _emailService.SendTemplatedEmailAsync(
                client.Email,
                client.Name,
                $"{recall.Producer.Name} procède au rappel de certains produits.",
                nameof(RecallSentEvent),
                recall.GetNotificationData(
                    _idSerializer.Serialize("Query", nameof(Domain.Recall), recall.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    recall.Comment,
                    recall.Producer.Name),
                true,
                token);
        }
    }
}
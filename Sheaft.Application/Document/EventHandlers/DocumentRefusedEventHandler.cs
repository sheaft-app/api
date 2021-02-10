using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Document;

namespace Sheaft.Application.Document.EventHandlers
{
    public class DocumentRefusedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DocumentRefusedEvent>>
    {
        public DocumentRefusedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DocumentRefusedEvent> notification, CancellationToken token)
        {
            var docEvent = notification.DomainEvent;
            var legal = await _context.GetSingleAsync<BusinessLegal>(l => l.Documents.Any(d => d.Id == docEvent.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(d => d.Id == docEvent.DocumentId);
            if (document.Status != DocumentStatus.Refused)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Document du producteur {legal.User.Name} refusé",
               $"Le document {document.Name} de type {document.Kind} du producteur {legal.User.Name} ({legal.User.Email}) a été refusé. Raison: {legal.Declaration.ReasonCode}-{legal.Declaration.ReasonMessage}.",
               false,
               token);
        }
    }
}
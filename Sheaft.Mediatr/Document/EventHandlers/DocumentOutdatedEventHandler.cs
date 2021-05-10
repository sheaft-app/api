using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Document;

namespace Sheaft.Mediatr.Document.EventHandlers
{
    public class DocumentEventsHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DocumentOutdatedEvent>>
    {
        public DocumentEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DocumentOutdatedEvent> notification, CancellationToken token)
        {
            var docEvent = notification.DomainEvent;
            var legal = await _context.Set<BusinessLegal>()
                .SingleOrDefaultAsync(l => l.Documents.Any(d => d.Id == docEvent.DocumentId), token);
            var document = legal.Documents.FirstOrDefault(d => d.Id == docEvent.DocumentId);
            if (document.Status != DocumentStatus.OutOfDate)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Document du producteur {legal.User.Name} expiré",
               $"Le document {document.Name} de type {document.Kind} du producteur {legal.User.Name} ({legal.User.Email}) est expiré. Raison: {legal.Declaration.ReasonCode}-{legal.Declaration.ReasonMessage}.",
               false,
               token);
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class DocumentEventsHandler : EventsHandler,
        INotificationHandler<DocumentRefusedEvent>,
        INotificationHandler<DocumentOutdatedEvent>
    {
        public DocumentEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DocumentRefusedEvent docEvent, CancellationToken token)
        {
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

        public async Task Handle(DocumentOutdatedEvent docEvent, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(l => l.Documents.Any(d => d.Id == docEvent.DocumentId), token);
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
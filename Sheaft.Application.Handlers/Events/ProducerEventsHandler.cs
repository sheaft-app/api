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
    public class ProducerEventsHandler : EventsHandler,
        INotificationHandler<ProducerDeclarationNotValidatedEvent>,
        INotificationHandler<ProducerDocumentsNotValidatedEvent>
    {
        public ProducerEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public async Task Handle(ProducerDeclarationNotValidatedEvent notification, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == notification.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La validation de la configuration de la déclaration du producteur {legal.User.Name} a échouée",
               $"La validation de la configuration de la déclaration du producteur {legal.User.Name} ({legal.User.Email}) a échouée. Veuillez vérifier la configuration de la déclaration depuis le site https://manage.sheaft.com.",
               false,
               token);
        }

        public async Task Handle(ProducerDocumentsNotValidatedEvent notification, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == notification.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La validation de la configuration des documents du producteur {legal.User.Name} a échouée",
               $"La validation de la configuration des documents du producteur {legal.User.Name} ({legal.User.Email}) a échouée. Veuillez vérifier la configuration des documents depuis le site https://manage.sheaft.com.",
               false,
               token);
        }
    }
}
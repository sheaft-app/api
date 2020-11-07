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
        INotificationHandler<ProducerDocumentsNotValidatedEvent>,
        INotificationHandler<ProducerRegisteredEvent>
    {
        public ProducerEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
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

        public async Task Handle(ProducerRegisteredEvent notification, CancellationToken token)
        {
            var producer = await _context.GetSingleAsync<Producer>(c => c.Id == notification.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Nouveau producteur sur la plateforme",
               $"Un nouveau producteur ({producer.Name}) ({producer.Address.Zipcode}) vient de s'enregistrer sur la plateforme, vous pouvez le contacter par email ({producer.Email}) ou par téléphone ({producer.Phone}).",
               false,
               token);
        }
    }
}
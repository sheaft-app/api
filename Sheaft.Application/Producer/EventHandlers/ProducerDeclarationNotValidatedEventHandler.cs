using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class ProducerDeclarationNotValidatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProducerDeclarationNotValidatedEvent>>
    {
        public ProducerDeclarationNotValidatedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<ProducerDeclarationNotValidatedEvent> notification, CancellationToken token)
        {
            var producerEvent = notification.DomainEvent;
            var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == producerEvent.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La validation de la configuration de la déclaration du producteur {legal.User.Name} a échouée",
               $"La validation de la configuration de la déclaration du producteur {legal.User.Name} ({legal.User.Email}) a échouée. Veuillez vérifier la configuration de la déclaration depuis le site https://manage.sheaft.com.",
               false,
               token);
        }
    }
}
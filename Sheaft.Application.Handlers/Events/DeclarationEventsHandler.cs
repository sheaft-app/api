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
    public class DeclarationEventsHandler : EventsHandler,
        INotificationHandler<DeclarationIncompleteEvent>,
        INotificationHandler<DeclarationRefusedEvent>,
        INotificationHandler<DeclarationValidatedEvent>
    {
        public DeclarationEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
        }

        public async Task Handle(DeclarationIncompleteEvent declarationEvent, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(l => l.Declaration.Id == declarationEvent.DeclarationId, token);
            if (legal.Declaration.Status != DeclarationStatus.Incomplete)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Déclaration UBO du producteur {legal.User.Name} incomplète",
               $"La déclaration d'ubo du producteur {legal.User.Name} ({legal.User.Email}) est incomplète. Raison: {legal.Declaration.ReasonCode}-{legal.Declaration.ReasonMessage}.",
               false,
               token);
        }

        public async Task Handle(DeclarationRefusedEvent declarationEvent, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(l => l.Declaration.Id == declarationEvent.DeclarationId, token);
            if (legal.Declaration.Status != DeclarationStatus.Refused)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Déclaration UBO du producteur {legal.User.Name} refusée",
               $"La déclaration d'ubo du producteur {legal.User.Name} ({legal.User.Email}) a été refusée. Raison: {legal.Declaration.ReasonCode}-{legal.Declaration.ReasonMessage}.",
               false,
               token);
        }

        public Task Handle(DeclarationValidatedEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models.Mailer;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class PreAuthorizationEventsHandler : EventsHandler,
        INotificationHandler<PreAuthorizationFailedEvent>,
        INotificationHandler<PreAuthorizationSucceededEvent>
    {
        private readonly IConfiguration _configuration;

        public PreAuthorizationEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(PreAuthorizationFailedEvent preAuthorizationEvent, CancellationToken token)
        {
            var preAuthorization = await _context.GetByIdAsync<PreAuthorization>(preAuthorizationEvent.PreAuthorizationId, token);
            if (preAuthorization.Status != PreAuthorizationStatus.Failed)
                return;

            var preAuthorizationData = GetObject(preAuthorization);
            
            await _signalrService.SendNotificationToUserAsync(preAuthorization.Order.User.Id, nameof(PreAuthorizationFailedEvent), preAuthorizationData);
            await _emailService.SendTemplatedEmailAsync(
                preAuthorization.Order.User.Email,
                preAuthorization.Order.User.Name,
                $"Votre paiement de {preAuthorization.Debited}€ a échoué",
                nameof(PreAuthorizationFailedEvent),
                preAuthorizationData,
                true,
                token);
        }        

        public async Task Handle(PreAuthorizationSucceededEvent preAuthorizationEvent, CancellationToken token)
        {
            var preAuthorization = await _context.GetByIdAsync<PreAuthorization>(preAuthorizationEvent.PreAuthorizationId, token);
            if (preAuthorization.Status != PreAuthorizationStatus.Succeeded)
                return;

            var preAuthorizationData = GetObject(preAuthorization);
            
            await _signalrService.SendNotificationToUserAsync(preAuthorization.Order.User.Id, nameof(PreAuthorizationSucceededEvent), preAuthorizationData);
            await _emailService.SendTemplatedEmailAsync(
                preAuthorization.Order.User.Email,
                preAuthorization.Order.User.Name,
                $"Votre paiement de {preAuthorization.Debited}€ a été validé",
                nameof(PreAuthorizationSucceededEvent),
                preAuthorizationData,
                true,
                token);
        }

        private OrderMailerModel GetObject(PreAuthorization preAuthorization)
        {
            return new OrderMailerModel { 
                FirstName = preAuthorization.Order.User.FirstName, 
                LastName = preAuthorization.Order.User.LastName, 
                TotalPrice = preAuthorization.Order.TotalPrice, 
                CreatedOn = preAuthorization.Order.CreatedOn, 
                ProductsCount = preAuthorization.Order.ProductsCount, 
                Reference = preAuthorization.Order.Reference, 
                OrderId = preAuthorization.Order.Id, 
                MyOrdersUrl = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{preAuthorization.Order.Id:N}"
            };
        }
    }
}
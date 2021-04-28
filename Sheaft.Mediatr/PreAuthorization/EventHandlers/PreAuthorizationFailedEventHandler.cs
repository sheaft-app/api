using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;

namespace Sheaft.Mediatr.PreAuthorization.EventHandlers
{
    public class PreAuthorizationFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PreAuthorizationFailedEvent>>
    {
        private readonly IConfiguration _configuration;
        
        public PreAuthorizationFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PreAuthorizationFailedEvent> customEvent, CancellationToken token)
        {
            var preAuthorizationEvent = customEvent.DomainEvent;
            var preAuthorization = await _context.PreAuthorizations.SingleAsync(e => e.Id == preAuthorizationEvent.PreAuthorizationId, token);
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

        private OrderMailerModel GetObject(Domain.PreAuthorization preAuthorization)
        {
            return new OrderMailerModel { 
                FirstName = preAuthorization.Order.User.FirstName, 
                LastName = preAuthorization.Order.User.LastName, 
                TotalPrice = preAuthorization.Order.TotalPrice, 
                CreatedOn = preAuthorization.Order.CreatedOn, 
                ProductsCount = preAuthorization.Order.ProductsCount, 
                Reference = preAuthorization.Order.Reference, 
                OrderId = preAuthorization.OrderId, 
                MyOrdersUrl = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{preAuthorization.OrderId:N}"
            };
        }
    }
}
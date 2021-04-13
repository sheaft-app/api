using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;

namespace Sheaft.Mediatr.PreAuthorization.EventHandlers
{
    public class PreAuthorizationSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PreAuthorizationSucceededEvent>>
    {
        private readonly IConfiguration _configuration;
        
        public PreAuthorizationSucceededEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        } 

        public async Task Handle(DomainEventNotification<PreAuthorizationSucceededEvent> customEvent, CancellationToken token)
        {
            var preAuthorizationEvent = customEvent.DomainEvent;
            var preAuthorization = await _context.GetByIdAsync<Domain.PreAuthorization>(preAuthorizationEvent.PreAuthorizationId, token);
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

        private OrderMailerModel GetObject(Domain.PreAuthorization preAuthorization)
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
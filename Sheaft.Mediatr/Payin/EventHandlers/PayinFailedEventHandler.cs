using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Payin;

namespace Sheaft.Mediatr.Payin.EventHandlers
{
    public class PayinFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PayinFailedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PayinFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PayinFailedEvent> notification, CancellationToken token)
        {
            var payinEvent = notification.DomainEvent;
            var payin = await _context.GetByIdAsync<Domain.Payin>(payinEvent.PayinId, token);
            if (payin.Status != TransactionStatus.Failed)
                return;

            var payinData = GetObject(payin);
            await _signalrService.SendNotificationToUserAsync(payin.Author.Id, nameof(PayinFailedEvent), payinData);
            await _emailService.SendTemplatedEmailAsync(
                payin.Order.User.Email,
                payin.Order.User.Name,
                $"Votre paiement de {payin.Debited}€ a échoué",
                nameof(PayinFailedEvent),
                payinData,
                true,
                token);
        }

        private OrderMailerModel GetObject(Domain.Payin payin)
        {
            return new OrderMailerModel { 
                FirstName = payin.Order.User.FirstName, 
                LastName = payin.Order.User.LastName, 
                TotalPrice = payin.Order.TotalPrice, 
                CreatedOn = payin.Order.CreatedOn, 
                ProductsCount = payin.Order.ProductsCount, 
                Reference = payin.Order.Reference, 
                OrderId = payin.Order.Id, 
                MyOrdersUrl = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{payin.Order.Id:N}"
            };
        }
    }
}
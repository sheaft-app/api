﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Application.Models.Mailer;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
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
            var payin = await _context.GetByIdAsync<Payin>(payinEvent.PayinId, token);
            if (payin.Order.Payin.Id != payin.Id)
                return;

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

        private OrderMailerModel GetObject(Payin payin)
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Handlers
{
    public class PurchaseOrderEventsHandler : EventsHandler,
        INotificationHandler<PurchaseOrderAcceptedEvent>,
        INotificationHandler<PurchaseOrderWithdrawnEvent>,
        INotificationHandler<PurchaseOrderCancelledEvent>,
        INotificationHandler<PurchaseOrderCompletedEvent>,
        INotificationHandler<PurchaseOrderProcessingEvent>,
        INotificationHandler<PurchaseOrderCreatedEvent>,
        INotificationHandler<PurchaseOrderReceivedEvent>,
        INotificationHandler<PurchaseOrderRefusedEvent>,
        INotificationHandler<CreateTransferFailedEvent>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(PurchaseOrderAcceptedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderAcceptedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var email = purchaseOrder.Sender.Email;
            var name = purchaseOrder.Sender.Name;
            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}";

            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{purchaseOrder.Vendor.Name} a accepté votre commande",
                nameof(PurchaseOrderAcceptedEvent),
                GetTemplateDatas(purchaseOrder, url),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderWithdrawnEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, nameof(PurchaseOrderWithdrawnEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"{purchaseOrder.Sender.Name} a annulé sa commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderWithdrawnEvent),
                GetTemplateDatas(purchaseOrder, url),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderCancelledEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCancelledEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"{purchaseOrder.Vendor.Name} a annulé votre commande",
                nameof(PurchaseOrderCancelledEvent),
                GetTemplateDatas(purchaseOrder, $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderCompletedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCompletedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"Votre commande est prête",
                nameof(PurchaseOrderCompletedEvent),
                GetTemplateDatas(purchaseOrder, $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderCreatedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCreatedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"Votre commande a été envoyée à {purchaseOrder.Vendor.Name}",
                nameof(PurchaseOrderCreatedEvent),
                GetTemplateDatas(purchaseOrder, $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderReceivedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Vendor.Id, nameof(PurchaseOrderReceivedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"{purchaseOrder.Sender.Name} a envoyé une nouvelle commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderReceivedEvent),
                GetTemplateDatas(purchaseOrder, $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderRefusedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderRefusedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"Votre commande a été annulée par {purchaseOrder.Vendor.Name}",
                nameof(PurchaseOrderRefusedEvent),
                GetTemplateDatas(purchaseOrder, $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        public async Task Handle(PurchaseOrderProcessingEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderProcessingEvent), GetPurchaseNotifModelAsString(purchaseOrder));
        }

        public async Task Handle(CreateTransferFailedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _emailService.SendEmailAsync(
                "support@sheaft.com",
                "Support",
                $"La création du transfert pour la commande {purchaseOrder.Reference} a échoué",
                $"La création du transfert de {purchaseOrder.TotalOnSalePrice}€ pour la commande {purchaseOrder.Reference} de {purchaseOrder.Vendor.Name} a échoué.",
                false,
                token);
        }

        private PurchaseOrderMailerModel GetTemplateDatas(PurchaseOrder purchaseOrder, string url)
        {
            var senderName = purchaseOrder.Sender.Name;
            var lines = purchaseOrder.Products.Select(o => new PurchaseOrderLineMailerModel 
            { 
                Line_Name = o.Name, 
                Line_Quantity = o.Quantity, 
                Line_Price = o.TotalOnSalePrice 
            }).ToList();

            var address = purchaseOrder.ExpectedDelivery.Address;
            return new PurchaseOrderMailerModel 
            { 
                Lines = lines, 
                SenderName = senderName,
                Reference = purchaseOrder.Reference, 
                VendorName = purchaseOrder.Vendor.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                TotalOnSalePrice = purchaseOrder.TotalOnSalePrice, 
                PurchaseOrderUrl = url,
                VatPrice = purchaseOrder.TotalVatPrice,
                ExpectedFrom = purchaseOrder.ExpectedDelivery.From,
                ExpectedTo = purchaseOrder.ExpectedDelivery.To,
                Address = address != null ? $"{address.Line1} {address.Line2} {address.Zipcode} {address.City}" : null,
                Reason = purchaseOrder.Reason
            };
        }

        private static string GetPurchaseNotifModelAsString(PurchaseOrder purchaseOrder)
        {
            return JsonConvert.SerializeObject(GetPurchaseNotifModel(purchaseOrder));
        }

        private static object GetPurchaseNotifModel(PurchaseOrder purchaseOrder)
        {
            return new
            {
                PurchaseOrderId = purchaseOrder.Id,
                Status = purchaseOrder.Status,
                Reference = purchaseOrder.Reference,
                VendorName = purchaseOrder.Vendor.Name,
                SenderName = purchaseOrder.Sender.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate
            };
        }
    }
}
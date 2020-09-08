using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class PurchaseOrderEventsHandler :
        INotificationHandler<PurchaseOrderAcceptedEvent>,
        INotificationHandler<PurchaseOrderCancelledBySenderEvent>,
        INotificationHandler<PurchaseOrderCancelledByVendorEvent>,
        INotificationHandler<PurchaseOrderCompletedEvent>,
        INotificationHandler<PurchaseOrderProcessingEvent>,
        INotificationHandler<PurchaseOrderCreatedEvent>,
        INotificationHandler<PurchaseOrderRefusedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISignalrService _signalrService;
        private readonly IConfiguration _configuration;

        public PurchaseOrderEventsHandler(IConfiguration configuration, IAppDbContext context, IEmailService emailService, ISignalrService signalrService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _signalrService = signalrService;
        }

        public async Task Handle(PurchaseOrderAcceptedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderAcceptedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var email = purchaseOrder.Sender.Email;
            var name = purchaseOrder.Sender.Name;
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/my-orders/{purchaseOrder.Id}";

            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                PurchaseOrderAcceptedEvent.MAILING_TEMPLATE_ID,
                GetTemplateDatas(purchaseOrder, url),
                token);
        }

        public async Task Handle(PurchaseOrderCancelledBySenderEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, nameof(PurchaseOrderCancelledBySenderEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                PurchaseOrderCancelledBySenderEvent.MAILING_TEMPLATE_ID,
                GetTemplateDatas(purchaseOrder, url),
                token);
        }

        public async Task Handle(PurchaseOrderCancelledByVendorEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCancelledByVendorEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/my-orders/{purchaseOrder.Id}";

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                PurchaseOrderCancelledByVendorEvent.MAILING_TEMPLATE_ID,
                GetTemplateDatas(purchaseOrder, url),
                token);
        }

        public async Task Handle(PurchaseOrderCompletedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCompletedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var email = purchaseOrder.Sender.Email;
            var name = purchaseOrder.Sender.Name;
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/my-orders/{purchaseOrder.Id}";

            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                PurchaseOrderCompletedEvent.MAILING_TEMPLATE_ID,
                GetTemplateDatas(purchaseOrder, url),
                token);
        }

        public async Task Handle(PurchaseOrderCreatedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, nameof(PurchaseOrderCreatedEvent), GetPurchaseNotifModelAsString(purchaseOrder));
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCreatedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                PurchaseOrderCreatedEvent.MAILING_TEMPLATE_ID_VENDOR,
                GetTemplateDatas(purchaseOrder, url),
                token);

            var senderMail = purchaseOrder.Sender.Email;
            var senderName = purchaseOrder.Sender.Name;
            
            await _emailService.SendTemplatedEmailAsync(
                senderMail,
                senderName,
                PurchaseOrderCreatedEvent.MAILING_TEMPLATE_ID_SENDER,
                GetTemplateDatas(purchaseOrder, url),
                token);
        }

        public async Task Handle(PurchaseOrderRefusedEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderRefusedEvent), GetPurchaseNotifModelAsString(purchaseOrder));

            var email = purchaseOrder.Sender.Email;
            var name = purchaseOrder.Sender.Name;
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/my-orders/{purchaseOrder.Id}";

            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                PurchaseOrderRefusedEvent.MAILING_TEMPLATE_ID,
                GetTemplateDatas(purchaseOrder, url),
                token);
        }

        public async Task Handle(PurchaseOrderProcessingEvent orderEvent, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.Id, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderProcessingEvent), GetPurchaseNotifModelAsString(purchaseOrder));
        }

        private object GetTemplateDatas(Domain.Models.PurchaseOrder purchaseOrder, string url)
        {
            var senderName = purchaseOrder.Sender.Name;
            var lines = purchaseOrder.Products.Select(o => new { Line_Name = o.Name, Line_Quantity = o.Quantity, Line_Price = o.TotalOnSalePrice }).ToList();
            return new { Lines = lines, SenderName = senderName, purchaseOrder.Reference, VendorName = purchaseOrder.Vendor.Name, purchaseOrder.CreatedOn, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate, purchaseOrder.TotalOnSalePrice, PurchaseOrderUrl = url };
        }

        private static string GetPurchaseNotifModelAsString(Domain.Models.PurchaseOrder purchaseOrder)
        {
            return JsonConvert.SerializeObject(GetPurchaseNotifModel(purchaseOrder));
        }

        private static object GetPurchaseNotifModel(Domain.Models.PurchaseOrder purchaseOrder)
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
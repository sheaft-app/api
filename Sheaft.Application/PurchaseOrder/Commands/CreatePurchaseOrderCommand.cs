using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class CreatePurchaseOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public Guid ProducerId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class CreatePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>
    {
        private readonly IIdentifierService _identifierService;
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public CreatePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<CreatePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == producer.Id);

            var resultIdentifier =
                await _identifierService.GetNextPurchaseOrderReferenceAsync(request.ProducerId, token);
            if (!resultIdentifier.Succeeded)
                return Failure<Guid>(resultIdentifier.Exception);

            var purchaseOrder = order.AddPurchaseOrder(resultIdentifier.Data, producer);
            await _context.SaveChangesAsync(token);

            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _capingDeliveriesService.IncreaseProducerDeliveryCountAsync(producer.Id, delivery.Id,
                    purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate, purchaseOrder.ExpectedDelivery.From,
                    purchaseOrder.ExpectedDelivery.To, delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            if (delivery.DeliveryMode.AutoAcceptRelatedPurchaseOrder)
                _mediatr.Post(new AcceptPurchaseOrderCommand(request.RequestUser)
                    {PurchaseOrderId = purchaseOrder.Id, SkipNotification = request.SkipNotification});

            return Success(purchaseOrder.Id);
        }
    }
}
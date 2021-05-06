using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class CreatePurchaseOrderCommand : Command<Guid>
    {
        protected CreatePurchaseOrderCommand()
        {
            
        }
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
        private readonly ITableService _tableService;

        public CreatePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            ITableService tableService,
            ILogger<CreatePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _tableService = tableService;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.ProducerId == producer.Id);

            var resultIdentifier =
                await _identifierService.GetNextPurchaseOrderReferenceAsync(request.ProducerId, token);
            if (!resultIdentifier.Succeeded)
                return Failure<Guid>(resultIdentifier);

            var purchaseOrder = order.AddPurchaseOrder(resultIdentifier.Data, producer);
            await _context.SaveChangesAsync(token);

            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _tableService.IncreaseProducerDeliveryCountAsync(producer.Id, delivery.DeliveryModeId,
                    purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate, purchaseOrder.ExpectedDelivery.From,
                    purchaseOrder.ExpectedDelivery.To, delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            if (delivery.DeliveryMode.AutoAcceptRelatedPurchaseOrder)
                _mediatr.Post(new AcceptPurchaseOrderCommand(request.RequestUser)
                    {PurchaseOrderId = purchaseOrder.Id, SkipNotification = false});

            return Success(purchaseOrder.Id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class CreatePurchaseOrderFromOrderCommand : Command<Guid>
    {
        protected CreatePurchaseOrderFromOrderCommand()
        {
            
        }
        [JsonConstructor]
        public CreatePurchaseOrderFromOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public Guid ProducerId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class CreatePurchaseOrderFromOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreatePurchaseOrderFromOrderCommand, Result<Guid>>
    {
        private readonly IIdentifierService _identifierService;
        private readonly ITableService _tableService;
        private readonly RoleOptions _roles;

        public CreatePurchaseOrderFromOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            ITableService tableService,
            IOptionsSnapshot<RoleOptions> roles,
            ILogger<CreatePurchaseOrderFromOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _tableService = tableService;
            _roles = roles.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderFromOrderCommand request, CancellationToken token)
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
                _mediatr.Post(new AcceptPurchaseOrderCommand(new RequestUser{Email = producer.Email, Id = producer.Id, Name = producer.Name, Roles = new List<string>{_roles.Producer.Value, _roles.Owner.Value}})
                    {PurchaseOrderId = purchaseOrder.Id, SkipNotification = false});

            return Success(purchaseOrder.Id);
        }
    }
}
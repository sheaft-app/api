using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Transfer.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class DeliverPurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public DeliverPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            SkipNotification = false;
        }

        public Guid PurchaseOrderId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class DeliverPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeliverPurchaseOrderCommand, Result>
    {
        public DeliverPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<DeliverPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if(purchaseOrder.Vendor.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            purchaseOrder.Deliver(request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var producerLegals =
                await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == purchaseOrder.Vendor.Id, token);
            _mediatr.Schedule(new CreatePurchaseOrderTransferCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id},
                TimeSpan.FromDays(7));

            return Success();
        }
    }
}
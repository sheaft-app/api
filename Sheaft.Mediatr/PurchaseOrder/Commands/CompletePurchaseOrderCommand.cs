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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Donation.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class CompletePurchaseOrderCommand : Command
    {
        protected CompletePurchaseOrderCommand()
        {
            
        }
        [JsonConstructor]
        public CompletePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class CompletePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CompletePurchaseOrderCommand, Result>
    {
        public CompletePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CompletePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if(purchaseOrder.VendorId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            purchaseOrder.Complete(request.SkipNotification);

            var order = await _context.Orders.SingleOrDefaultAsync(
                o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);

            if (order.Donation > 0)
            {
                var dateDiff = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate.AddDays(7) - DateTime.UtcNow;
                _mediatr.Schedule(new CreateDonationCommand(request.RequestUser) {OrderId = order.Id},
                    TimeSpan.FromDays(dateDiff.TotalDays));
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
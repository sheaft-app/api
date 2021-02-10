using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Transfer.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class DeliverPurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public DeliverPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            SkipNotification = false;
        }

        public Guid Id { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class DeliverPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeliverPurchaseOrderCommand, Result>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public DeliverPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<DeliverPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.Id, token);
            purchaseOrder.Deliver(request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var producerLegals =
                await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == purchaseOrder.Vendor.Id, token);
            _mediatr.Schedule(new CreateTransferCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id},
                TimeSpan.FromDays(7));

            return Success();
        }
    }
}
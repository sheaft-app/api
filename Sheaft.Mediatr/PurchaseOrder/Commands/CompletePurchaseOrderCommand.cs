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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Delivery.Commands;
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
        private readonly IIdentifierService _identifierService;

        public CompletePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            ILogger<CompletePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if(purchaseOrder.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            purchaseOrder.Complete(request.SkipNotification);

            var order = await _context.Orders.SingleOrDefaultAsync(
                o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);

            if (order.Donation > 0)
            {
                var dateDiff = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate.AddDays(7) - DateTime.UtcNow;
                _mediatr.Schedule(new CreateDonationCommand(request.RequestUser) {OrderId = order.Id},
                    TimeSpan.FromDays(dateDiff.TotalDays));
            }

            if ((int) purchaseOrder.ExpectedDelivery.Kind <= 4)
            {
                var producer = await _context.Producers.SingleAsync(p => p.Id == purchaseOrder.ProducerId, token);
                var user = await _context.Users.SingleAsync(p => p.Id == purchaseOrder.ClientId, token);
                
                var identifier = await _identifierService.GetNextDeliveryReferenceAsync(producer.Id, token);
                if (!identifier.Succeeded)
                    return Failure(identifier);
                
                var delivery = new Domain.Delivery(identifier.Data, producer, purchaseOrder.ExpectedDelivery.Kind, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate, purchaseOrder.ExpectedDelivery.Address, user.Id, user.Name, new []{purchaseOrder}, 0);
                delivery.SetAsReady();
                
                await _context.AddAsync(delivery, token);
                _mediatr.Post(new GenerateDeliveryFormCommand(request.RequestUser) {DeliveryId = delivery.Id});
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Core.Enums;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Picking.Commands
{
    public class UpdatePickingCommand : Command
    {
        protected UpdatePickingCommand()
        {
        }

        [JsonConstructor]
        public UpdatePickingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }

    public class UpdatePickingCommandHandler : CommandsHandler,
        IRequestHandler<UpdatePickingCommand, Result>
    {
        public UpdatePickingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdatePickingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdatePickingCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");

            var existingPurchaseOrderIds = picking.PurchaseOrders.Select(po => po.Id);
            var purchaseOrderIdsToRemove = existingPurchaseOrderIds.Except(request.PurchaseOrderIds);
            var purchaseOrdersToRemove =
                picking.PurchaseOrders.Where(po => purchaseOrderIdsToRemove.Contains(po.Id));
            
            picking.RemovePurchaseOrders(purchaseOrdersToRemove);
            existingPurchaseOrderIds = picking.PurchaseOrders.Select(po => po.Id);

            var purchaseOrderIdsToAdd = request.PurchaseOrderIds.Except(existingPurchaseOrderIds);
            var purchaseOrdersToAdd = await _context.PurchaseOrders
                .Where(p => purchaseOrderIdsToAdd.Contains(p.Id))
                .ToListAsync(token);
            
            if(purchaseOrdersToAdd.Any())
                picking.AddPurchaseOrders(purchaseOrdersToAdd);
            
            picking.SetName(request.Name);

            await _context.SaveChangesAsync(token);
            
            if(picking.Status == PickingStatus.InProgress)
                _mediatr.Post(new GeneratePickingFormCommand(request.RequestUser) {PickingId = picking.Id});
            
            return Success();
        }
    }
}
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
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Core.Enums;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Picking.Commands
{
    public class SetPickingProductPreparedQuantityCommand : Command
    {
        protected SetPickingProductPreparedQuantityCommand()
        {
        }

        [JsonConstructor]
        public SetPickingProductPreparedQuantityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
        public Guid ProductId { get; set; }
        public IEnumerable<PickingPurchaseOrderProductQuantityDto> Quantities { get; set; }
        public IEnumerable<Guid> Batches { get; set; }
        public string PreparedBy { get; set; }
        public bool Completed { get; set; }
    }

    public class SetPickingProductPreparedQuantityCommandHandler : CommandsHandler,
        IRequestHandler<SetPickingProductPreparedQuantityCommand, Result>
    {
        public SetPickingProductPreparedQuantityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetPickingProductPreparedQuantityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetPickingProductPreparedQuantityCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");

            if (picking.Status != PickingStatus.InProgress)
                return Failure(
                    "Impossible de mettre à jour la quantité d'un produit d'une préparation qui n'est pas en cours de traitement.");

            var preparedBy = string.IsNullOrWhiteSpace(request.PreparedBy)
                ? picking.Producer.Name
                : request.PreparedBy;

            if (request.Quantities == null || !request.Quantities.Any())
                return Failure("Les quantités préparées pour le produit sont requises.");
            
            var batches = request.Batches != null && request.Batches.Any() ? await _context.Batches
                .Where(b => request.Batches.Contains(b.Id))
                .ToListAsync(token) : new List<Domain.Batch>();
            
            foreach (var quantity in request.Quantities)
                picking.SetProductPreparedQuantity(request.ProductId, quantity.PurchaseOrderId,
                    quantity.PreparedQuantity, preparedBy, request.Completed, batches);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
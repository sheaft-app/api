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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Mediatr.Recall.Commands
{
    public class UpdateRecallCommand : Command
    {
        protected UpdateRecallCommand()
        {
        }

        [JsonConstructor]
        public UpdateRecallCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RecallId { get; set; }
        public IEnumerable<Guid> BatchIds { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset SaleStartedOn { get; set; }
        public DateTimeOffset SaleEndedOn { get; set; }
    }

    public class UpdateRecallCommandHandler : CommandsHandler,
        IRequestHandler<UpdateRecallCommand, Result>
    {
        public UpdateRecallCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateRecallCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateRecallCommand request, CancellationToken token)
        {
            var recall = await _context.Recalls.SingleOrDefaultAsync(b => b.Id == request.RecallId, token);
            if (recall == null)
                return Failure("La campagne de rappel est introuvable.");
            
            if (request.SaleStartedOn > request.SaleEndedOn)
                return Failure("La date de fin de commercialisation ne peut pas être antérieure à la date de début de commercialisation.");

            var batches = request.BatchIds != null
                ? await _context.Batches
                    .Where(b => request.BatchIds.Contains(b.Id))
                    .ToListAsync(token)
                : new List<Domain.Batch>();
            
            var products = request.ProductIds != null
                ? await _context.Products
                    .Where(b => request.ProductIds.Contains(b.Id))
                    .ToListAsync(token)
                : new List<Domain.Product>();
            
            var clientIds = await _context.PurchaseOrders
                .Where(po =>
                    po.Picking.PreparedProducts.Any(pp => pp.PurchaseOrderId == po.Id && request.ProductIds.Contains(pp.ProductId)) ||
                    po.Picking.PreparedProducts.Any(pp => pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => request.BatchIds.Contains(b.BatchId))))
                .Select(po => po.ClientId)
                .ToListAsync(token);

            var clients = await _context.Users
                .Where(u => clientIds.Contains(u.Id))
                .ToListAsync(token);
            
            recall.SetName(request.Name);
            recall.SetComment(request.Comment);
            recall.SetBatches(batches);
            recall.SetProducts(products);
            recall.SetClients(clients);
            recall.SetSaleStartedOn(request.SaleStartedOn);
            recall.SetSaleEndedOn(request.SaleEndedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
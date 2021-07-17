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
using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Mediatr.Observation.Commands
{
    public class UpdateObservationCommand : Command
    {
        protected UpdateObservationCommand()
        {
        }

        [JsonConstructor]
        public UpdateObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ObservationId { get; set; }
        public IEnumerable<Guid> BatchIds { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
        public string Comment { get; set; }
        public bool VisibleToAll { get; set; }
    }

    public class UpdateObservationCommandHandler : CommandsHandler,
        IRequestHandler<UpdateObservationCommand, Result>
    {
        public UpdateObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateObservationCommand request, CancellationToken token)
        {
            if (request.BatchIds == null || !request.BatchIds.Any() && request.ProductIds == null ||
                !request.ProductIds.Any())
                return Failure<Guid>("Vous devez préciser au moins un produit ou un lot pour faire une observation.");
            
            var observation = await _context.Observations.SingleOrDefaultAsync(b => b.Id == request.ObservationId, token);
            if (observation == null)
                return Failure("L'observation est introuvable.");
            
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

            observation.SetBatches(batches);
            observation.SetProducts(products);
            observation.SetComment(request.Comment);
            observation.SetVisibility(request.VisibleToAll);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Observation.Commands
{
    public class CreateObservationCommand : Command<Guid>
    {
        protected CreateObservationCommand()
        {
        }

        [JsonConstructor]
        public CreateObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> BatchIds { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
        public string Comment { get; set; }
        public bool VisibleToAll { get; set; }
    }

    public class CreateObservationCommandHandler : CommandsHandler,
        IRequestHandler<CreateObservationCommand, Result<Guid>>
    {
        public CreateObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateObservationCommand request, CancellationToken token)
        {
            if ((request.BatchIds == null || !request.BatchIds.Any()) && (request.ProductIds == null || !request.ProductIds.Any()))
                return Failure<Guid>("Vous devez préciser au moins un produit ou un lot pour faire une observation.");
            
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

            var batchProducerIds = batches.Select(b => b.ProducerId).Distinct().ToList();
            if (batchProducerIds.Count > 1)
                return Failure<Guid>("Une observation est liée à un producteur, les lots doivent donc appartenir au même producteur.");

            var productProducerIds = products.Select(b => b.ProducerId).Distinct().ToList();
            if (productProducerIds.Count > 1)
                return Failure<Guid>("Une observation est liée à un producteur, les produits doivent donc appartenir au même producteur.");

            var producerId = productProducerIds.Any() ? productProducerIds.First() : batchProducerIds.First();
            var producer = await _context.Producers.SingleOrDefaultAsync(p => p.Id == producerId, token);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.RequestUser.Id, token);
            
            var entity = new Domain.Observation(Guid.NewGuid(), request.Comment, user, producer);
            entity.SetBatches(batches);
            entity.SetProducts(products);
            entity.SetVisibility(request.VisibleToAll);    
            
            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}
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

namespace Sheaft.Mediatr.Recall.Commands
{
    public class CreateRecallCommand : Command<Guid>
    {
        protected CreateRecallCommand()
        {
        }

        [JsonConstructor]
        public CreateRecallCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = requestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public IEnumerable<Guid> BatchIds { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset SaleStartedOn { get; set; }
        public DateTimeOffset SaleEndedOn { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = user.Id;
        }
    }

    public class CreateRecallCommandHandler : CommandsHandler,
        IRequestHandler<CreateRecallCommand, Result<Guid>>
    {
        public CreateRecallCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateRecallCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateRecallCommand request, CancellationToken token)
        {
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

            var producer = await _context.Producers.SingleOrDefaultAsync(p => p.Id == request.ProducerId, token);

            var entity = new Domain.Recall(Guid.NewGuid(), request.Name, request.SaleStartedOn, request.SaleEndedOn, request.Comment, producer, products);
            entity.SetBatches(batches);
            
            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}
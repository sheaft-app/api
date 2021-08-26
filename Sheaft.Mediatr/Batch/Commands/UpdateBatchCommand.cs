using System;
using System.Collections.Generic;
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
using Sheaft.Domain.Common;

namespace Sheaft.Mediatr.Batch.Commands
{
    public class UpdateBatchCommand : Command
    {
        protected UpdateBatchCommand()
        {
        }

        [JsonConstructor]
        public UpdateBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchId { get; set; }
        public string Number { get; set; }
        public DateTimeOffset? DLC { get; set; }
        public DateTimeOffset? DDM { get; set; }
        public IEnumerable<BatchField> Fields { get; set; }
    }

    public class UpdateBatchCommandHandler : CommandsHandler,
        IRequestHandler<UpdateBatchCommand, Result>
    {
        public UpdateBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken token)
        {
            var entity = await _context.Batches.SingleOrDefaultAsync(b => b.Id == request.BatchId, token);
            if (entity == null)
                return Failure("Le lot est introuvable.");

            if (request.Number != entity.Number)
            {
                var existingBatchWithNumber =
                    await _context.Batches.SingleOrDefaultAsync(
                        b => b.Number == request.Number && b.ProducerId == entity.ProducerId, token);
                
                if (existingBatchWithNumber != null)
                    return Failure("Un lot existe déjà avec ce numéro.");
            }

            if (await _context.Set<PreparedProductBatch>().AnyAsync(ppb => ppb.BatchId == entity.Id, token))
                return Failure("Impossible de modifier un lot qui est déjà rattaché à des produits.");
            
            entity.SetNumber(request.Number);
            entity.SetDLC(request.DLC);
            entity.SetDDM(request.DDM);
            entity.SetValues(request.Fields);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
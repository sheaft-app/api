using System;
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

namespace Sheaft.Mediatr.Batch.Commands
{
    public class DeleteBatchCommand : Command
    {
        protected DeleteBatchCommand()
        {
        }

        [JsonConstructor]
        public DeleteBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchId { get; set; }
    }

    public class DeleteBatchCommandHandler : CommandsHandler,
        IRequestHandler<DeleteBatchCommand, Result>
    {
        public DeleteBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteBatchCommand request, CancellationToken token)
        {
            var entity = await _context.Batches.SingleOrDefaultAsync(b => b.Id == request.BatchId, token);
            if (entity == null)
                return Failure("Le lot est introuvable.");

            var batchUsed = await _context.Set<PreparedProductBatch>().AnyAsync(pb => pb.BatchId == request.BatchId, token);
            if(batchUsed)
                return Failure("Impossible de supprimer ce lot, il est associé à des produits préparés/livrés.");
            
            _context.Remove(entity);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
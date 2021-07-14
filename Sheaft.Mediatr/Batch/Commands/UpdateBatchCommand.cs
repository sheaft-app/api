using System;
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
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

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
        public DateTimeOffset? DLUO { get; set; }
        public string Comment { get; set; }
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
            
            entity.SetNumber(request.Number);
            entity.SetDLC(request.DLC);
            entity.SetDLUO(request.DLUO);
            entity.SetComment(request.Comment);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
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
    public class RestoreBatchCommand : Command
    {
        protected RestoreBatchCommand()
        {
        }

        [JsonConstructor]
        public RestoreBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchId { get; set; }
    }

    public class RestoreBatchCommandHandler : CommandsHandler,
        IRequestHandler<RestoreBatchCommand, Result>
    {
        public RestoreBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreBatchCommand request, CancellationToken token)
        {
            var entity = await _context.Batches.SingleOrDefaultAsync(b => b.Id == request.BatchId, token);
            if (entity == null)
                return Failure("Le lot est introuvable.");

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
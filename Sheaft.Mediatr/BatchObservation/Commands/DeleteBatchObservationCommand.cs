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

namespace Sheaft.Mediatr.BatchObservation.Commands
{
    public class DeleteBatchObservationCommand : Command
    {
        protected DeleteBatchObservationCommand()
        {
        }

        [JsonConstructor]
        public DeleteBatchObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchObservationId { get; set; }
        public string Comment { get; set; }
    }

    public class DeleteBatchObservationCommandHandler : CommandsHandler,
        IRequestHandler<DeleteBatchObservationCommand, Result>
    {
        public DeleteBatchObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteBatchObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteBatchObservationCommand request, CancellationToken token)
        {
            var batch = await _context.Batches.SingleOrDefaultAsync(b => b.Observations.Any(o => o.Id == request.BatchObservationId), token);
            if (batch == null)
                return Failure("Le lot est introuvable.");

            batch.RemoveObservation(request.BatchObservationId);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
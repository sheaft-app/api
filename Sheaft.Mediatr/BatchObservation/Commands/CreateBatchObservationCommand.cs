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
    public class CreateBatchObservationCommand : Command
    {
        protected CreateBatchObservationCommand()
        {
        }

        [JsonConstructor]
        public CreateBatchObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchId { get; set; }
        public string Comment { get; set; }
    }

    public class CreateBatchObservationCommandHandler : CommandsHandler,
        IRequestHandler<CreateBatchObservationCommand, Result>
    {
        public CreateBatchObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateBatchObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CreateBatchObservationCommand request, CancellationToken token)
        {
            var batch = await _context.Batches.SingleOrDefaultAsync(b => b.Id == request.BatchId, token);
            if (batch == null)
                return Failure("Le lot est introuvable.");

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.RequestUser.Id, token);
            batch.AddObservation(request.Comment, user);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
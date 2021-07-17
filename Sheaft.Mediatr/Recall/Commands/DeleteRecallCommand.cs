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

namespace Sheaft.Mediatr.Recall.Commands
{
    public class DeleteRecallCommand : Command
    {
        protected DeleteRecallCommand()
        {
        }

        [JsonConstructor]
        public DeleteRecallCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RecallId { get; set; }
    }

    public class DeleteRecallCommandHandler : CommandsHandler,
        IRequestHandler<DeleteRecallCommand, Result>
    {
        public DeleteRecallCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteRecallCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteRecallCommand request, CancellationToken token)
        {
            var recall = await _context.Recalls.SingleOrDefaultAsync(b => b.Id == request.RecallId, token);
            if (recall == null)
                return Failure("La campagne de rappel est introuvable.");

            _context.Remove(recall);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
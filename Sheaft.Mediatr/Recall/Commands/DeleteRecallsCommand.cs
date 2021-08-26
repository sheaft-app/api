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
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Recall.Commands
{
    public class DeleteRecallsCommand : Command
    {
        protected DeleteRecallsCommand()
        {
        }

        [JsonConstructor]
        public DeleteRecallsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> RecallIds { get; set; }
    }

    public class DeleteRecallsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteRecallsCommand, Result>
    {
        public DeleteRecallsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteRecallsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteRecallsCommand request, CancellationToken token)
        {
            var recalls = await _context.Recalls
                .Where(b => request.RecallIds.Contains(b.Id))
                .ToListAsync(token);
            
            if (recalls == null || !recalls.Any())
                return Failure("Les campagnes de rappel sont introuvables.");

            foreach (var recall in recalls)
            {
                if(recall.Status != RecallStatus.Waiting && recall.Status != RecallStatus.Ready)
                    throw SheaftException.BadRequest("Impossible de supprimer une campagne qui a été envoyée.");
                
                _context.Remove(recall);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
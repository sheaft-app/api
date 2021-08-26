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

namespace Sheaft.Mediatr.BatchDefinition.Commands
{
    public class DeleteBatchDefinitionCommand : Command
    {
        protected DeleteBatchDefinitionCommand()
        {
        }

        [JsonConstructor]
        public DeleteBatchDefinitionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchDefinitionId { get; set; }
    }

    public class DeleteBatchDefinitionCommandHandler : CommandsHandler,
        IRequestHandler<DeleteBatchDefinitionCommand, Result>
    {
        public DeleteBatchDefinitionCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteBatchDefinitionCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteBatchDefinitionCommand request, CancellationToken token)
        {
            var batchDefinition = await _context.BatchDefinitions.SingleOrDefaultAsync(b => b.Id == request.BatchDefinitionId, token);
            if (batchDefinition == null)
                return Failure("La définition du lot est introuvable.");

            _context.Remove(batchDefinition);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
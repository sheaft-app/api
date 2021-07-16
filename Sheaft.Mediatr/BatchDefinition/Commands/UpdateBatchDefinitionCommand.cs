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

namespace Sheaft.Mediatr.BatchDefinition.Commands
{
    public class UpdateBatchDefinitionCommand : Command
    {
        protected UpdateBatchDefinitionCommand()
        {
        }

        [JsonConstructor]
        public UpdateBatchDefinitionCommand(RequestUser requestUser) : base(requestUser)
        {
        }


        public Guid BatchDefinitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<BatchField> FieldDefinitions { get; set; }
    }

    public class UpdateBatchDefinitionCommandHandler : CommandsHandler,
        IRequestHandler<UpdateBatchDefinitionCommand, Result>
    {
        public UpdateBatchDefinitionCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateBatchDefinitionCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateBatchDefinitionCommand request, CancellationToken token)
        {
            var batchDefinition = await _context.BatchDefinitions.SingleOrDefaultAsync(b => b.Id == request.BatchDefinitionId, token);
            if (batchDefinition == null)
                return Failure("Le lot est introuvable.");

            batchDefinition.SetName(request.Name);
            batchDefinition.SetIsDefault(request.IsDefault);
            batchDefinition.SetDescription(request.Description);
            batchDefinition.SetFields(request.FieldDefinitions);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
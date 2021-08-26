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
    public class CreateBatchCommand : Command<Guid>
    {
        protected CreateBatchCommand()
        {
        }

        [JsonConstructor]
        public CreateBatchCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = requestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string Number { get; set; }
        public DateTimeOffset? DLC { get; set; }
        public DateTimeOffset? DDM { get; set; }
        public IEnumerable<BatchField> Fields { get; set; }
        public Guid? DefinitionId { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = user.Id;
        }
    }

    public class CreateBatchCommandHandler : CommandsHandler,
        IRequestHandler<CreateBatchCommand, Result<Guid>>
    {
        public CreateBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateBatchCommand request, CancellationToken token)
        {
            var existingBatchWithNumber =
                await _context.Batches.SingleOrDefaultAsync(
                    b => b.Number == request.Number && b.ProducerId == request.ProducerId, token);
            if (existingBatchWithNumber != null)
                return Failure<Guid>("Un lot existe déjà avec ce numéro.");

            var producer = await _context.Producers.SingleAsync(p => p.Id == request.ProducerId, token);
            var batchDefinition = request.DefinitionId.HasValue ? await _context.BatchDefinitions.SingleOrDefaultAsync(b => b.Id == request.DefinitionId, token) : await _context.BatchDefinitions.SingleOrDefaultAsync(
                b => b.IsDefault && b.ProducerId == producer.Id, token);

            if (batchDefinition == null)
                return Failure<Guid>("La définition est introuvable.");

            var entity = new Domain.Batch(Guid.NewGuid(), request.Number, producer, batchDefinition);

            entity.SetDLC(request.DLC);
            entity.SetDDM(request.DDM);
            entity.SetValues(request.Fields);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
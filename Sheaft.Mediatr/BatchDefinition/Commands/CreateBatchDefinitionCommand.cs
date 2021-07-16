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
    public class CreateBatchDefinitionCommand : Command<Guid>
    {
        protected CreateBatchDefinitionCommand()
        {
        }

        [JsonConstructor]
        public CreateBatchDefinitionCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = requestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<BatchField> FieldDefinitions { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = user.Id;
        }
    }

    public class CreateBatchDefinitionCommandHandler : CommandsHandler,
        IRequestHandler<CreateBatchDefinitionCommand, Result<Guid>>
    {
        public CreateBatchDefinitionCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateBatchDefinitionCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateBatchDefinitionCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleOrDefaultAsync(u => u.Id == request.RequestUser.Id, token);
            
            var entity = new Domain.BatchDefinition(Guid.NewGuid(), request.Name, producer, request.FieldDefinitions);
            entity.SetIsDefault(request.IsDefault);
            entity.SetDescription(request.Description);
            
            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}
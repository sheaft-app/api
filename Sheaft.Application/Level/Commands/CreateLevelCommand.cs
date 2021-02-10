using Sheaft.Domain.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CreateLevelCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }

    public class CreateLevelCommandHandler : CommandsHandler,
        IRequestHandler<CreateLevelCommand, Result<Guid>>
    {
        public CreateLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = new Level(Guid.NewGuid(), request.Name, request.RequiredPoints);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }
    }
}

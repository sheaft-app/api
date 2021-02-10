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
    public class UpdateLevelCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }

    public class UpdateLevelCommandHandler : CommandsHandler,
        IRequestHandler<UpdateLevelCommand, Result<bool>>
    {
        public UpdateLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Level>(request.Id, token);
                entity.SetName(request.Name);
                entity.SetRequiredPoints(request.RequiredPoints);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

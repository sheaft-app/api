using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Level.Commands
{
    public class RestoreLevelCommand : Command
    {
        [JsonConstructor]
        public RestoreLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class RestoreLevelCommandHandler : CommandsHandler,
        IRequestHandler<RestoreLevelCommand, Result>
    {
        public RestoreLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreLevelCommand request, CancellationToken token)
        {
            var entity = await _context.Levels.SingleOrDefaultAsync(r => r.Id == request.Id, token);
            _context.Restore(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Level.Commands
{
    public class RestoreLevelCommand : Command
    {
        protected RestoreLevelCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LevelId { get; set; }
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
            var entity = await _context.Levels.SingleOrDefaultAsync(r => r.Id == request.LevelId, token);
            _context.Restore(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
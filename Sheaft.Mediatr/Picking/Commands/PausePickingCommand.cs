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

namespace Sheaft.Mediatr.Picking.Commands
{
    public class PausePickingCommand : Command
    {
        protected PausePickingCommand()
        {
        }
        
        [JsonConstructor]
        public PausePickingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
    }

    public class PausePickingCommandHandler : CommandsHandler,
        IRequestHandler<PausePickingCommand, Result>
    {
        public PausePickingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<PausePickingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(PausePickingCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");

            picking.Pause();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
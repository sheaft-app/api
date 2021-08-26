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
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Picking.Commands
{
    public class StartPickingCommand : Command
    {
        protected StartPickingCommand()
        {
        }

        [JsonConstructor]
        public StartPickingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
    }

    public class StartPickingCommandHandler : CommandsHandler,
        IRequestHandler<StartPickingCommand, Result>
    {
        public StartPickingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartPickingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartPickingCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");

            var oldStatus = picking.Status;
            
            picking.Start();
            await _context.SaveChangesAsync(token);

            if(oldStatus == PickingStatus.Waiting)
                _mediatr.Post(new GeneratePickingFormCommand(request.RequestUser) {PickingId = picking.Id});

            return Success();
        }
    }
}
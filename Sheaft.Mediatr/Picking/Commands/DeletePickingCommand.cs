using System;
using System.Linq;
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
    public class DeletePickingCommand : Command
    {
        protected DeletePickingCommand()
        {
        }

        [JsonConstructor]
        public DeletePickingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
    }

    public class DeletePickingCommandHandler : CommandsHandler,
        IRequestHandler<DeletePickingCommand, Result>
    {
        public DeletePickingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeletePickingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeletePickingCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");
            
            picking.RemovePurchaseOrders(picking.PurchaseOrders.ToList());

            _context.Remove(picking);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
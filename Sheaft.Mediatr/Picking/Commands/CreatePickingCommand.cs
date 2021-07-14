using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Core.Enums;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Picking.Commands
{
    public class CreatePickingCommand : Command<Guid>
    {
        protected CreatePickingCommand()
        {
        }

        [JsonConstructor]
        public CreatePickingCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = requestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = user.Id;
        }
    }

    public class CreatePickingCommandHandler : CommandsHandler,
        IRequestHandler<CreatePickingCommand, Result<Guid>>
    {
        public CreatePickingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreatePickingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreatePickingCommand request, CancellationToken token)
        {
            var purchaseOrders = await _context.PurchaseOrders
                .Where(p => request.PurchaseOrderIds.Contains(p.Id))
                .ToListAsync(token);

            var producer = await _context.Producers.SingleAsync(p => p.Id == request.ProducerId, token);
            var entity = new Domain.Picking(Guid.NewGuid(), request.Name, producer, purchaseOrders);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
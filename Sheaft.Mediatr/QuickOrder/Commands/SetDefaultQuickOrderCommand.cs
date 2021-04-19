using System;
using System.Linq;
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
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class SetDefaultQuickOrderCommand : Command
    {
        [JsonConstructor]
        public SetDefaultQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }
        public Guid QuickOrderId { get; set; }
    }

    public class SetDefaultQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<SetDefaultQuickOrderCommand, Result>
    {
        public SetDefaultQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDefaultQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetDefaultQuickOrderCommand request, CancellationToken token)
        {
            var quickOrders = await _context.QuickOrders.Where(c => c.User.Id == request.UserId).ToListAsync(token);
            var entity = quickOrders.FirstOrDefault(qo => qo.Id == request.QuickOrderId);
            if (entity == null)
                return Failure(SheaftException.NotFound());
            
            entity.SetAsDefault();

            var oldDefaultQuickOrder = quickOrders.SingleOrDefault(qo => qo.Id != request.QuickOrderId && qo.IsDefault);
            if (oldDefaultQuickOrder != null)
                oldDefaultQuickOrder.UnsetAsDefault();
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
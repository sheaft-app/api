using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Order.Commands
{
    public class ResetOrderCommand : Command
    {
        [JsonConstructor]
        public ResetOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class ResetOrderCommandHandler : CommandsHandler,
        IRequestHandler<ResetOrderCommand, Result>
    {
        public ResetOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<ResetOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ResetOrderCommand request, CancellationToken token)
        {
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            if (order.User.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            if (order.Status != OrderStatus.Refused && order.Status != OrderStatus.Waiting && order.Status != OrderStatus.Expired)
                return Failure(MessageKind.BadRequest);

            var pendingPayins = await _context.Payins
                .Where(p => p.Order.Id == request.OrderId)
                .ToListAsync(token);

            if (pendingPayins.Any(p => p.Status == TransactionStatus.Succeeded))
                return Failure(MessageKind.BadRequest);

            foreach (var pendingPayin in pendingPayins)
            {
                pendingPayin.SetStatus(TransactionStatus.Cancelled);
            }
            
            var pendingPreAuthorizations = await _context.PreAuthorizations
                .Where(p => p.Order.Id == request.OrderId)
                .ToListAsync(token);

            if (pendingPreAuthorizations.Any(p => p.Status == PreAuthorizationStatus.Succeeded))
                return Failure(MessageKind.BadRequest);

            foreach (var pendingPreAuthorization in pendingPreAuthorizations)
            {
                pendingPreAuthorization.SetStatus(PreAuthorizationStatus.Cancelled);
            }
                
            order.SetStatus(OrderStatus.Created);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
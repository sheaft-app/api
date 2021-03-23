using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class SetDefaultQuickOrderCommand : Command
    {
        [JsonConstructor]
        public SetDefaultQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
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
            var quickOrders = await _context.FindAsync<Domain.QuickOrder>(c => c.User.Id == request.UserId, token);
            foreach (var quickOrder in quickOrders)
            {
                if (quickOrder.Id == request.QuickOrderId)
                    quickOrder.SetAsDefault();
                else
                    quickOrder.UnsetAsDefault();
            }

            _context.UpdateRange(quickOrders);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
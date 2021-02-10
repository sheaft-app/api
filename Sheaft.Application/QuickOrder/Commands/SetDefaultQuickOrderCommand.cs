using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.QuickOrder.Commands
{
    public class SetDefaultQuickOrderCommand : Command
    {
        [JsonConstructor]
        public SetDefaultQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
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
            var quickOrders = await _context.FindAsync<Domain.QuickOrder>(c => c.User.Id == request.RequestUser.Id, token);
            foreach (var quickOrder in quickOrders)
            {
                if (quickOrder.Id == request.Id)
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
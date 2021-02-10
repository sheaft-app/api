using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class SetDefaultQuickOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public SetDefaultQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class SetDefaultQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<SetDefaultQuickOrderCommand, Result<bool>>
    {
        public SetDefaultQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDefaultQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(SetDefaultQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var quickOrders = await _context.FindAsync<QuickOrder>(c => c.User.Id == request.RequestUser.Id, token);
                foreach (var quickOrder in quickOrders)
                {
                    if (quickOrder.Id == request.Id)
                        quickOrder.SetAsDefault();
                    else
                        quickOrder.UnsetAsDefault();
                }

                _context.UpdateRange(quickOrders);
                await _context.SaveChangesAsync(token); 

                return Ok(true);
            });
        }
    }
}

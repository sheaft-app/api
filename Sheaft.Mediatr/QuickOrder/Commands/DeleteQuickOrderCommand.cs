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

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class DeleteQuickOrderCommand : Command
    {
        protected DeleteQuickOrderCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid QuickOrderId { get; set; }
    }

    public class DeleteQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeleteQuickOrderCommand, Result>
    {
        public DeleteQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteQuickOrderCommand request, CancellationToken token)
        {
            var entity = await _context.QuickOrders.SingleAsync(e => e.Id == request.QuickOrderId, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
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
    public class DeleteQuickOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeleteQuickOrderCommand, Result<bool>>
    {
        public DeleteQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}

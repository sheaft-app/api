using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class DeleteRewardCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteRewardCommandHandler : CommandsHandler,
        IRequestHandler<DeleteRewardCommand, Result<bool>>
    {
        public DeleteRewardCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteRewardCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(DeleteRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Reward>(request.Id, token);
                
                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}

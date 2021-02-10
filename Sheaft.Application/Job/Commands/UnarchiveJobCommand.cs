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
    public class UnarchiveJobCommand : Command<bool>
    {
        [JsonConstructor]
        public UnarchiveJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class UnarchiveJobCommandHandler : CommandsHandler,
        IRequestHandler<UnarchiveJobCommand, Result<bool>>
    {
        public UnarchiveJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<UnarchiveJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        
        public async Task<Result<bool>> Handle(UnarchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.UnarchiveJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

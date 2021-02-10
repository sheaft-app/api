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
    public class PauseJobCommand : Command<bool>
    {
        [JsonConstructor]
        public PauseJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class PauseJobCommandHandler : CommandsHandler,
        IRequestHandler<PauseJobCommand, Result<bool>>
    {
        public PauseJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<PauseJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(PauseJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.PauseJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

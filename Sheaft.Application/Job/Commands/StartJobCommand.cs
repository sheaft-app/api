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
    public class StartJobCommand : Command<bool>
    {
        [JsonConstructor]
        public StartJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class StartJobCommandHandler : CommandsHandler,
        IRequestHandler<StartJobCommand, Result<bool>>
    {
        public StartJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<StartJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(StartJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.StartJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

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
    public class FailJobCommand : Command<bool>
    {
        [JsonConstructor]
        public FailJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
    
    public class FailJobCommandHandler : CommandsHandler,
        IRequestHandler<FailJobCommand, Result<bool>>
    {
        public FailJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<FailJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(FailJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.FailJob(request.Reason);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

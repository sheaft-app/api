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
    public class CancelJobCommand : Command<bool>
    {
        [JsonConstructor]
        public CancelJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
    
    public class CancelJobCommandHandler : CommandsHandler,
        IRequestHandler<CancelJobCommand, Result<bool>>
    {
        public CancelJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<CancelJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(CancelJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.CancelJob(request.Reason);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

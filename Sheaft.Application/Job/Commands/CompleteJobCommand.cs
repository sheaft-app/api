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
    public class CompleteJobCommand : Command<bool>
    {
        [JsonConstructor]
        public CompleteJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string FileUrl { get; set; }
    }
    
    public class CompleteJobCommandHandler : CommandsHandler,
        IRequestHandler<CompleteJobCommand, Result<bool>>
    {
        public CompleteJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<CompleteJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CompleteJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.SetDownloadUrl(request.FileUrl);
                entity.CompleteJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

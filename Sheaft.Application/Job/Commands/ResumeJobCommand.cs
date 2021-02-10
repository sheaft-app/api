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
    public class ResumeJobCommand : Command<bool>
    {
        [JsonConstructor]
        public ResumeJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class ResumeJobCommandHandler : CommandsHandler,
        IRequestHandler<ResumeJobCommand, Result<bool>>
    {
        public ResumeJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<ResumeJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(ResumeJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.ResumeJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

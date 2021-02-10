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
    public class DeleteJobCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class DeleteJobCommandHandler : CommandsHandler,
        IRequestHandler<DeleteJobCommand, Result<bool>>
    {
        public DeleteJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<DeleteJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

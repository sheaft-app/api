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
    public class DeleteTagCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteTagCommandHandler : CommandsHandler,
        IRequestHandler<DeleteTagCommand, Result<bool>>
    {
        public DeleteTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(DeleteTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

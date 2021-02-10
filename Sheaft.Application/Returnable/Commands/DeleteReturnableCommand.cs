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
    public class DeleteReturnableCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteReturnableCommandHandler : CommandsHandler,
        IRequestHandler<DeleteReturnableCommand, Result<bool>>
    {
        public DeleteReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Returnable>(request.Id, token);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}

using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class DeleteLevelCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class DeleteLevelCommandHandler : CommandsHandler,
        IRequestHandler<DeleteLevelCommand, Result<bool>>
    {
        public DeleteLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Level>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

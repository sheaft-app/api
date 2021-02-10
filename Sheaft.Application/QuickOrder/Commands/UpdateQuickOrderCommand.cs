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
    public class UpdateQuickOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<UpdateQuickOrderCommand, Result<bool>>
    {
        public UpdateQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateQuickOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<QuickOrder>(request.Id, token);
                entity.SetName(request.Name);
                entity.SetDescription(request.Name);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

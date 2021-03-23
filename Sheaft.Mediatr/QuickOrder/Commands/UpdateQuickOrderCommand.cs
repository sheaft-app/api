using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class UpdateQuickOrderCommand : Command
    {
        [JsonConstructor]
        public UpdateQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid QuickOrderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<UpdateQuickOrderCommand, Result>
    {
        public UpdateQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateQuickOrderCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.QuickOrder>(request.QuickOrderId, token);
            entity.SetName(request.Name);
            entity.SetDescription(request.Name);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
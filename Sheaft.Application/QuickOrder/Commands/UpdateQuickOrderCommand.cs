using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.QuickOrder.Commands
{
    public class UpdateQuickOrderCommand : Command
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
            var entity = await _context.GetByIdAsync<Domain.QuickOrder>(request.Id, token);
            entity.SetName(request.Name);
            entity.SetDescription(request.Name);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
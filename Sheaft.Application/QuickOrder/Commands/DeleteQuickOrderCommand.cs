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
    public class DeleteQuickOrderCommand : Command
    {
        [JsonConstructor]
        public DeleteQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class DeleteQuickOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeleteQuickOrderCommand, Result>
    {
        public DeleteQuickOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteQuickOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteQuickOrderCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.QuickOrder>(request.Id, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
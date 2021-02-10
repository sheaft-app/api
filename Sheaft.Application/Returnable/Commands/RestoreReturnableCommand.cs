using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Returnable.Commands
{
    public class RestoreReturnableCommand : Command
    {
        [JsonConstructor]
        public RestoreReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class RestoreReturnableCommandHandler : CommandsHandler,
        IRequestHandler<RestoreReturnableCommand, Result>
    {
        public RestoreReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreReturnableCommand request, CancellationToken token)
        {
            var entity =
                await _context.Returnables.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
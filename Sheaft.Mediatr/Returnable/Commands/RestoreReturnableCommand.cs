using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Returnable.Commands
{
    public class RestoreReturnableCommand : Command
    {
        protected RestoreReturnableCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ReturnableId { get; set; }
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
                await _context.Returnables.SingleOrDefaultAsync(a => a.Id == request.ReturnableId && a.RemovedOn.HasValue, token);

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Tag.Commands
{
    public class RestoreTagCommand : Command
    {
        [JsonConstructor]
        public RestoreTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
    }
    
    public class RestoreTagCommandHandler : CommandsHandler,
        IRequestHandler<RestoreTagCommand, Result>
    {
        public RestoreTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreTagCommand request, CancellationToken token)
        {
                var entity = await _context.Tags.SingleOrDefaultAsync(a => a.Id == request.TagId && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Success();
        }
    }
}

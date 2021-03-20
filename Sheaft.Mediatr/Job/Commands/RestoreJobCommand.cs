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
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class RestoreJobCommand : Command
    {
        [JsonConstructor]
        public RestoreJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class RestoreJobCommandHandler : CommandsHandler,
        IRequestHandler<RestoreJobCommand, Result>
    {
        public RestoreJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreJobCommand request, CancellationToken token)
        {
            var entity =
                await _context.Jobs.SingleOrDefaultAsync(a => a.Id == request.JobId && a.RemovedOn.HasValue, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            _context.Restore(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
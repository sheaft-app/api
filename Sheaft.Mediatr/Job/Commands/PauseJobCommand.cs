using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class PauseJobCommand : Command
    {
        [JsonConstructor]
        public PauseJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class PauseJobCommandHandler : CommandsHandler,
        IRequestHandler<PauseJobCommand, Result>
    {
        public PauseJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<PauseJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(PauseJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.PauseJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
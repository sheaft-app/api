using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class PauseJobCommand : Command
    {
        protected PauseJobCommand()
        {
            
        }
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
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            entity.PauseJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
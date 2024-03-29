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
    public class CancelJobCommand : Command
    {
        protected CancelJobCommand()
        {
            
        }
        [JsonConstructor]
        public CancelJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public string Reason { get; set; }
    }

    public class CancelJobCommandHandler : CommandsHandler,
        IRequestHandler<CancelJobCommand, Result>
    {
        public CancelJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            entity.CancelJob(request.Reason);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class PauseJobsCommand : Command
    {
        [JsonConstructor]
        public PauseJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
    }

    public class PauseJobsCommandHandler : CommandsHandler,
        IRequestHandler<PauseJobsCommand, Result>
    {
        public PauseJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<PauseJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(PauseJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var jobId in request.JobIds)
                {
                    var result = await _mediatr.Process(new PauseJobCommand(request.RequestUser) {JobId = jobId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
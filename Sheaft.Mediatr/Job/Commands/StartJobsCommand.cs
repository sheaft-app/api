using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class StartJobsCommand : Command
    {
        [JsonConstructor]
        public StartJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
    }

    public class StartJobsCommandHandler : CommandsHandler,
        IRequestHandler<StartJobsCommand, Result>
    {
        public StartJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var jobId in request.JobIds)
                {
                    var result = await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = jobId}, token);
                    if (!result.Succeeded)
                        return Failure(result);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
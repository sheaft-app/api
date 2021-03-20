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
    public class RetryJobsCommand : Command
    {
        [JsonConstructor]
        public RetryJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
    }

    public class RetryJobsCommandHandler : CommandsHandler,
        IRequestHandler<RetryJobsCommand, Result>
    {
        public RetryJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RetryJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RetryJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var jobId in request.JobIds)
                {
                    var result = await _mediatr.Process(new RetryJobCommand(request.RequestUser) {JobId = jobId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
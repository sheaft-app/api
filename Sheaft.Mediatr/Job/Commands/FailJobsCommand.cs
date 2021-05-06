using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class FailJobsCommand : Command
    {
        protected FailJobsCommand()
        {
        }

        [JsonConstructor]
        public FailJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
        public string Reason { get; set; }
    }

    public class FailJobsCommandHandler : CommandsHandler,
        IRequestHandler<FailJobsCommand, Result>
    {
        public FailJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<FailJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(FailJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var jobId in request.JobIds)
                {
                    result =
                        await _mediatr.Process(
                            new FailJobCommand(request.RequestUser) {JobId = jobId, Reason = request.Reason}, token);
                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
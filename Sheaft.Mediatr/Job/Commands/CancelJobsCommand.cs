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
    public class CancelJobsCommand : Command
    {
        protected CancelJobsCommand()
        {
            
        }
        [JsonConstructor]
        public CancelJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
        public string Reason { get; set; }
    }

    public class CancelJobsCommandHandler : CommandsHandler,
        IRequestHandler<CancelJobsCommand, Result>
    {
        public CancelJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var jobId in request.JobIds)
                {
                    result = await _mediatr.Process(
                        new CancelJobCommand(request.RequestUser) {JobId = jobId, Reason = request.Reason}, token);
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
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

namespace Sheaft.Services.Job.Commands
{
    public class CancelJobsCommand : Command
    {
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
                foreach (var jobId in request.JobIds)
                {
                    var result = await _mediatr.Process(
                        new CancelJobCommand(request.RequestUser) {JobId = jobId, Reason = request.Reason}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
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
    public class ArchiveJobsCommand : Command
    {
        [JsonConstructor]
        public ArchiveJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
    }

    public class ArchiveJobsCommandHandler : CommandsHandler,
        IRequestHandler<ArchiveJobsCommand, Result>
    {
        public ArchiveJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ArchiveJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ArchiveJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var jobId in request.JobIds)
                {
                    var result = await _mediatr.Process(new ArchiveJobCommand(request.RequestUser) {JobId = jobId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
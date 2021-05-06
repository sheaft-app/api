using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
    public class ArchiveJobsCommand : Command
    {
        protected ArchiveJobsCommand()
        {
            
        }
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
                Result result = null;
                foreach (var jobId in request.JobIds)
                {
                    result = await _mediatr.Process(new ArchiveJobCommand(request.RequestUser) {JobId = jobId}, token);
                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded:false})
                    return result;

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
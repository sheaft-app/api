using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class PauseJobsCommand : Command
    {
        protected PauseJobsCommand()
        {
            
        }
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
                Result result = null;
                foreach (var jobId in request.JobIds)
                {
                    result = await _mediatr.Process(new PauseJobCommand(request.RequestUser) {JobId = jobId}, token);
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
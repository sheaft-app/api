using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class PauseJobsCommand : Command<bool>
    {
        [JsonConstructor]
        public PauseJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
    public class PauseJobsCommandHandler : CommandsHandler,
        IRequestHandler<PauseJobsCommand, Result<bool>>
    {
        public PauseJobsCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<PauseJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(PauseJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new PauseJobCommand(request.RequestUser) { Id = jobId }, token);
                       if (!result.Success)
                           return Failed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return Ok(true);
               }
           });
        }
    }
}

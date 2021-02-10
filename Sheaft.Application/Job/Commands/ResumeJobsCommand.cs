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
    public class ResumeJobsCommand : Command<bool>
    {
        [JsonConstructor]
        public ResumeJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
    public class ResumeJobsCommandHandler : CommandsHandler,
        IRequestHandler<ResumeJobsCommand, Result<bool>>
    {
        public ResumeJobsCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<ResumeJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        
        public async Task<Result<bool>> Handle(ResumeJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new ResumeJobCommand(request.RequestUser) { Id = jobId }, token);
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

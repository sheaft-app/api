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
    public class CancelJobsCommand : Command<bool>
    {
        [JsonConstructor]
        public CancelJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
    
    public class CancelJobsCommandHandler : CommandsHandler,
        IRequestHandler<CancelJobsCommand, Result<bool>>
    {
        public CancelJobsCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<CancelJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CancelJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new CancelJobCommand(request.RequestUser) { Id = jobId, Reason = request.Reason }, token);
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

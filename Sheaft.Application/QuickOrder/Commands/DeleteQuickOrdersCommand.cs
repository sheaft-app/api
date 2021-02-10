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
    public class DeleteQuickOrdersCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteQuickOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
    
    public class DeleteQuickOrdersCommandHandler : CommandsHandler,
        IRequestHandler<DeleteQuickOrdersCommand, Result<bool>>
    {
        public DeleteQuickOrdersCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteQuickOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteQuickOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                foreach (var id in request.Ids)
                {
                    var result = await _mediatr.Process(new DeleteQuickOrderCommand(request.RequestUser) { Id = id }, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);
                }

                return Ok(true);
            });
        }
    }
}

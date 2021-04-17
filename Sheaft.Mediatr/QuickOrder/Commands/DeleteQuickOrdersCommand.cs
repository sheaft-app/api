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

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class DeleteQuickOrdersCommand : Command
    {
        [JsonConstructor]
        public DeleteQuickOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> QuickOrderIds { get; set; }
    }

    public class DeleteQuickOrdersCommandHandler : CommandsHandler,
        IRequestHandler<DeleteQuickOrdersCommand, Result>
    {
        public DeleteQuickOrdersCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteQuickOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteQuickOrdersCommand request, CancellationToken token)
        {
            foreach (var id in request.QuickOrderIds)
            {
                var result = await _mediatr.Process(new DeleteQuickOrderCommand(request.RequestUser) {QuickOrderId = id}, token);
                if (!result.Succeeded)
                    return Failure(result);
            }

            return Success();
        }
    }
}
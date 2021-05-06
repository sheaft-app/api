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

namespace Sheaft.Mediatr.QuickOrder.Commands
{
    public class DeleteQuickOrdersCommand : Command
    {
        protected DeleteQuickOrdersCommand()
        {
            
        }
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
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var id in request.QuickOrderIds)
                {
                    result =
                        await _mediatr.Process(new DeleteQuickOrderCommand(request.RequestUser) {QuickOrderId = id},
                            token);
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
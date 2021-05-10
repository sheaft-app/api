using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class DeleteDeliveryClosingsCommand : Command
    {
        protected DeleteDeliveryClosingsCommand()
        {
        }

        [JsonConstructor]
        public DeleteDeliveryClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public List<Guid> ClosingIds { get; set; }
    }

    public class DeleteDeliveryClosingsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryClosingsCommand, Result>
    {
        public DeleteDeliveryClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var closingId in request.ClosingIds)
                {
                    result = await _mediatr.Process(
                        new DeleteDeliveryClosingCommand(request.RequestUser)
                            {ClosingId = closingId}, token);

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.DeliveryClosing.Commands
{
    public class DeleteDeliveryClosingsCommand : Command
    {
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
                foreach (var closingId in request.ClosingIds)
                {
                    var result = await _mediatr.Process(
                        new DeleteDeliveryClosingCommand(request.RequestUser)
                            {ClosingId = closingId}, token);
                    
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
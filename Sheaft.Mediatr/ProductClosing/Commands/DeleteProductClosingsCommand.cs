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

namespace Sheaft.Mediatr.ProductClosing.Commands
{
    public class DeleteProductClosingsCommand : Command
    {
        [JsonConstructor]
        public DeleteProductClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public List<Guid> ClosingIds { get; set; }
    }

    public class DeleteProductClosingsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductClosingsCommand, Result>
    {
        public DeleteProductClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteProductClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteProductClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var closingId in request.ClosingIds)
                {
                    var result = await _mediatr.Process(
                        new DeleteProductClosingCommand(request.RequestUser)
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
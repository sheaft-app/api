using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Services.ProductClosing.Commands
{
    public class DeleteProductClosingCommand : Command
    {
        [JsonConstructor]
        public DeleteProductClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ClosingId { get; set; }
    }

    public class DeleteProductClosingCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductClosingCommand, Result>
    {
        public DeleteProductClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteProductClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteProductClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetSingleAsync<Domain.Product>(c => c.Closings.Any(cc => cc.Id == request.ClosingId), token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.RemoveClosing(request.ClosingId);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
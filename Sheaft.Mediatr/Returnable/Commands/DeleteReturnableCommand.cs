using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Returnable.Commands
{
    public class DeleteReturnableCommand : Command
    {
        [JsonConstructor]
        public DeleteReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ReturnableId { get; set; }
    }

    public class DeleteReturnableCommandHandler : CommandsHandler,
        IRequestHandler<DeleteReturnableCommand, Result>
    {
        public DeleteReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteReturnableCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Returnable>(request.ReturnableId, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
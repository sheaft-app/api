using System;
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

namespace Sheaft.Application.Level.Commands
{
    public class DeleteLevelCommand : Command
    {
        [JsonConstructor]
        public DeleteLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LevelId { get; set; }
    }

    public class DeleteLevelCommandHandler : CommandsHandler,
        IRequestHandler<DeleteLevelCommand, Result>
    {
        public DeleteLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteLevelCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Level>(request.LevelId, token);
            _context.Remove(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
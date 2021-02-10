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
    public class UpdateLevelCommand : Command
    {
        [JsonConstructor]
        public UpdateLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }

    public class UpdateLevelCommandHandler : CommandsHandler,
        IRequestHandler<UpdateLevelCommand, Result>
    {
        public UpdateLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateLevelCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Level>(request.Id, token);
            entity.SetName(request.Name);
            entity.SetRequiredPoints(request.RequiredPoints);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
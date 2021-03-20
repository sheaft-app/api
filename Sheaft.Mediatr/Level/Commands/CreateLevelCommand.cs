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

namespace Sheaft.Mediatr.Level.Commands
{
    public class CreateLevelCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }

    public class CreateLevelCommandHandler : CommandsHandler,
        IRequestHandler<CreateLevelCommand, Result<Guid>>
    {
        public CreateLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateLevelCommand request, CancellationToken token)
        {
            var entity = new Domain.Level(Guid.NewGuid(), request.Name, request.RequiredPoints);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
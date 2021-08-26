using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Setting.Commands
{
    public class CreateSettingCommand : Command<Guid>
    {
        protected CreateSettingCommand()
        {
            
        }
        [JsonConstructor]
        public CreateSettingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public SettingKind Kind { get; set; }
    }

    public class CreateSettingCommandHandler : CommandsHandler,
        IRequestHandler<CreateSettingCommand, Result<Guid>>
    {
        public CreateSettingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateSettingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateSettingCommand request, CancellationToken token)
        {
            var entity = new Domain.Setting(Guid.NewGuid(), request.Name, request.Kind);
            entity.SetDescription(request.Description);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}
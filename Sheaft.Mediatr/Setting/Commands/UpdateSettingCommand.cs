using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Setting.Commands
{
    public class UpdateSettingCommand : Command
    {
        protected UpdateSettingCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateSettingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid SettingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SettingKind Kind { get; set; }
    }

    public class UpdateSettingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateSettingCommand, Result>
    {
        public UpdateSettingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateSettingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateSettingCommand request, CancellationToken token)
        {
            var entity = await _context.Settings.SingleAsync(e => e.Id == request.SettingId, token);

            entity.SetName(request.Name);
            entity.SetDescription(request.Description);
            entity.SetKind(request.Kind);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
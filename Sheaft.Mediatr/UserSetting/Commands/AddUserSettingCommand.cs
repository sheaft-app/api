using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.UserSetting.Commands
{
    public class AddUserSettingCommand : Command
    {
        [JsonConstructor]
        public AddUserSettingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Value { get; set; }
        public Guid UserId { get; set; }
        public Guid SettingId { get; set; }
    }

    public class AddUserSettingCommandHandler : CommandsHandler,
        IRequestHandler<AddUserSettingCommand, Result>
    {
        public AddUserSettingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AddUserSettingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AddUserSettingCommand request, CancellationToken token)
        {
            var setting = await _context.GetByIdAsync<Domain.Setting>(request.SettingId, token);
            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);

            user.AddSetting(setting, request.Value);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
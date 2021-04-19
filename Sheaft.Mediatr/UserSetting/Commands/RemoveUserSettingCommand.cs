using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.UserSetting.Commands
{
    public class RemoveUserSettingCommand : Command
    {
        [JsonConstructor]
        public RemoveUserSettingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public Guid SettingId { get; set; }
    }

    public class RemoveUserSettingCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserSettingCommand, Result>
    {
        public RemoveUserSettingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserSettingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserSettingCommand request, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            user.RemoveSetting(request.SettingId);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
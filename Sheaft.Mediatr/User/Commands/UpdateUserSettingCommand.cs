using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class UpdateUserSettingCommand : Command
    {
        protected UpdateUserSettingCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateUserSettingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Value { get; set; }
        public Guid UserId { get; set; }
        public Guid SettingId { get; set; }
    }

    public class UpdateUserSettingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateUserSettingCommand, Result>
    {
        public UpdateUserSettingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateUserSettingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateUserSettingCommand request, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            user.EditSetting(request.SettingId, request.Value);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
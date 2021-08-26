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

namespace Sheaft.Mediatr.Notification.Commands
{
    public class MarkUserNotificationsAsReadCommand : Command
    {
        protected MarkUserNotificationsAsReadCommand()
        {
            
        }
        [JsonConstructor]
        public MarkUserNotificationsAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public DateTimeOffset ReadBefore { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class MarkUserNotificationsAsReadCommandHandler : CommandsHandler,
        IRequestHandler<MarkUserNotificationsAsReadCommand, Result>
    {
        private readonly IDapperContext _dapperContext;

        public MarkUserNotificationsAsReadCommandHandler(
            IDapperContext dapperContext,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<MarkUserNotificationsAsReadCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Result> Handle(MarkUserNotificationsAsReadCommand request, CancellationToken token)
        {
            var result = await _dapperContext.SetNotificationAsReadAsync(request.UserId, request.ReadBefore, token);
            return result ? Success() : Failure("Une erreur est survenue pendant le marquage des notifications comme lues.");
        }
    }
}
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

namespace Sheaft.Mediatr.Notification.Commands
{
    public class MarkUserNotificationAsReadCommand : Command
    {
        protected MarkUserNotificationAsReadCommand()
        {
        }
        
        [JsonConstructor]
        public MarkUserNotificationAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid NotificationId { get; set; }
    }

    public class MarkUserNotificationAsReadCommandHandler : CommandsHandler,
        IRequestHandler<MarkUserNotificationAsReadCommand, Result>
    {
        public MarkUserNotificationAsReadCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<MarkUserNotificationAsReadCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(MarkUserNotificationAsReadCommand request, CancellationToken token)
        {
            var notification = await _context.Notifications.SingleAsync(e => e.Id == request.NotificationId, token);
            if (!notification.Unread)
                return Success();
            
            if(notification.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            notification.SetAsRead();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
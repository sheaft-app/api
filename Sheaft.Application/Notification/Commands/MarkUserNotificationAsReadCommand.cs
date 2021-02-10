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

namespace Sheaft.Application.Notification.Commands
{
    public class MarkUserNotificationAsReadCommand : Command
    {
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
            var notification = await _context.GetByIdAsync<Domain.Notification>(request.NotificationId, token);
            if (!notification.Unread)
                return Success();

            notification.SetAsRead();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
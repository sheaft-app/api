using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Notification.Commands
{
    public class CreateGroupNotificationCommand : Command<Guid>
    {
        protected CreateGroupNotificationCommand()
        {
            
        }
        [JsonConstructor]
        public CreateGroupNotificationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }

    public class CreateGroupNotificationCommandHandler : CommandsHandler,
        IRequestHandler<CreateGroupNotificationCommand, Result<Guid>>
    {
        public CreateGroupNotificationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateGroupNotificationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateGroupNotificationCommand request, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.GroupId, token);
            var entity = new Domain.Notification(Guid.NewGuid(), NotificationKind.Business, request.Method,
                request.Content,
                user);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
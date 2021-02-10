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
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Notification.Commands
{
    public class CreateGroupNotificationCommand : Command<Guid>
    {
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
            var user = await _context.GetByIdAsync<Domain.User>(request.GroupId, token);
            var entity = new Domain.Notification(Guid.NewGuid(), NotificationKind.Business, request.Method,
                request.Content,
                user);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
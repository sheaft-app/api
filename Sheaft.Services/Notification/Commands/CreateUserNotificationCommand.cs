using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Services.Notification.Commands
{
    public class CreateUserNotificationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateUserNotificationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }

    public class CreateUserNotificationCommandHandler : CommandsHandler,
        IRequestHandler<CreateUserNotificationCommand, Result<Guid>>
    {
        public CreateUserNotificationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateUserNotificationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateUserNotificationCommand request, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            var entity = new Domain.Notification(Guid.NewGuid(), NotificationKind.Business, request.Method,
                request.Content,
                user);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
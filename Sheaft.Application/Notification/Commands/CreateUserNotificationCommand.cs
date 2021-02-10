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
    public class CreateUserNotificationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateUserNotificationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }

    public class CreateUserNotificationCommandHandler : CommandsHandler,
        IRequestHandler<CreateUserNotificationCommand, Result<Guid>>
    {
        private readonly IDapperContext _dapperContext;

        public CreateUserNotificationCommandHandler(
            IDapperContext dapperContext,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateUserNotificationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Result<Guid>> Handle(CreateUserNotificationCommand request, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<Domain.User>(request.Id, token);
            var entity = new Domain.Notification(Guid.NewGuid(), NotificationKind.Business, request.Method, request.Content,
                user);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            return Success(entity.Id);
        }
    }
}
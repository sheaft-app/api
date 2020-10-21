using Sheaft.Application.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Commands;
using System;
using Sheaft.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class NotificationCommandsHandler : ResultsHandler,
        IRequestHandler<MarkUserNotificationsAsReadCommand, Result<bool>>,
        IRequestHandler<MarkUserNotificationAsReadCommand, Result<bool>>,
        IRequestHandler<CreateUserNotificationCommand, Result<Guid>>,
        IRequestHandler<CreateGroupNotificationCommand, Result<Guid>>
    {
        private readonly IDapperContext _dapperContext;

        public NotificationCommandsHandler(
            IDapperContext dapperContext,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<NotificationCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Result<bool>> Handle(MarkUserNotificationsAsReadCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result = await _dapperContext.SetNotificationAsReadAsync(request.RequestUser.Id, request.ReadBefore, token);
                return Ok(result);
            });
        }

        public async Task<Result<bool>> Handle(MarkUserNotificationAsReadCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var notification = await _context.GetByIdAsync<Notification>(request.Id, token);
                if(!notification.Unread)
                    return Ok(true);

                notification.SetAsRead();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(CreateUserNotificationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.Id, token);
                var entity = new Notification(Guid.NewGuid(), NotificationKind.Business, request.Method, request.Content, user);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }

        public async Task<Result<Guid>> Handle(CreateGroupNotificationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.Id, token);
                var entity = new Notification(Guid.NewGuid(), NotificationKind.Business, request.Method, request.Content, user);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }
    }
}
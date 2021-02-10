using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationAsReadCommand : Command<bool>
    {
        [JsonConstructor]
        public MarkUserNotificationAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class MarkUserNotificationAsReadCommandHandler : CommandsHandler,
        IRequestHandler<MarkUserNotificationAsReadCommand, Result<bool>>
    {
        private readonly IDapperContext _dapperContext;

        public MarkUserNotificationAsReadCommandHandler(
            IDapperContext dapperContext,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<MarkUserNotificationAsReadCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _dapperContext = dapperContext;
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
    }
}
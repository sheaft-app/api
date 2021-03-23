using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Notification.Commands
{
    public class MarkUserNotificationsAsReadCommand : Command
    {
        [JsonConstructor]
        public MarkUserNotificationsAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public DateTimeOffset ReadBefore { get; set; }
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
            return result ? Success() : Failure();
        }
    }
}
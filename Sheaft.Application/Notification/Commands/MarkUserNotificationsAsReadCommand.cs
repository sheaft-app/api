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
    public class MarkUserNotificationsAsReadCommand : Command
    {
        [JsonConstructor]
        public MarkUserNotificationsAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
        }

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
            var result = await _dapperContext.SetNotificationAsReadAsync(request.RequestUser.Id, request.ReadBefore, token);
            return result ? Success() : Failure();
        }
    }
}
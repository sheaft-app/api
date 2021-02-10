using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class MarkUserNotificationsAsReadCommand : Command<bool>
    {
        [JsonConstructor]
        public MarkUserNotificationsAsReadCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public DateTimeOffset ReadBefore { get; set; }
    }
    
    public class MarkUserNotificationsAsReadCommandHandler : CommandsHandler,
        IRequestHandler<MarkUserNotificationsAsReadCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(MarkUserNotificationsAsReadCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result = await _dapperContext.SetNotificationAsReadAsync(request.RequestUser.Id, request.ReadBefore, token);
                return Ok(result);
            });
        }
    }
}
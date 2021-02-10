using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckPayinCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
    
    public class CheckPayinCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CheckPayinCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckPayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }
        
        public async Task<Result<bool>> Handle(CheckPayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetByIdAsync<Payin>(request.PayinId, token);
                if (payin.Status != TransactionStatus.Created && payin.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result = await _mediatr.Process(new RefreshPayinStatusCommand(request.RequestUser, payin.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }
    }
}

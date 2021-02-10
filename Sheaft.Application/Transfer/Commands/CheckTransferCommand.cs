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
    public class CheckTransferCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }
    
    public class CheckTransferCommandHandler : CommandsHandler,
        IRequestHandler<CheckTransferCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CheckTransferCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckTransferCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var transfer = await _context.GetByIdAsync<Transfer>(request.TransferId, token);
                if (transfer.Status != TransactionStatus.Created && transfer.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result = await _mediatr.Process(new RefreshTransferStatusCommand(request.RequestUser, transfer.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }
    }
}

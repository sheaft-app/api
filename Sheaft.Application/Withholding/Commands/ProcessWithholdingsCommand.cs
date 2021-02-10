using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Withholding.Commands
{
    public class ProcessWithholdingsCommand : Command
    {
        [JsonConstructor]
        public ProcessWithholdingsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }

    public class ProcessWithholdingsCommandHandler : CommandsHandler,
        IRequestHandler<ProcessWithholdingsCommand, Result>
    {
        public ProcessWithholdingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ProcessWithholdingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ProcessWithholdingsCommand request, CancellationToken token)
        {
            var payout = await _context.GetByIdAsync<Domain.Payout>(request.PayoutId, token);
            if (payout.Status != TransactionStatus.Succeeded)
                return Failure(MessageKind.Withholding_Cannot_Process_Already_Succeeded);

            if (!payout.Withholdings.Any())
                return Failure(MessageKind.Withholding_Cannot_Process_Payout_No_Withholdings);

            foreach (var withholding in payout.Withholdings)
            {
                var result =
                    await _mediatr.Process(
                        new ProcessWithholdingCommand(request.RequestUser) {WithholdingId = withholding.Id}, token);
                if (!result.Succeeded)
                    throw result.Exception;
            }

            return Success();
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Withholding.Commands
{
    public class ProcessWithholdingsCommand : Command
    {
        protected ProcessWithholdingsCommand()
        {
            
        }
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
            var payout = await _context.Payouts.SingleAsync(e => e.Id == request.PayoutId, token);
            if (payout.Status != TransactionStatus.Succeeded)
                return Failure(MessageKind.Withholding_Cannot_Process_Already_Succeeded);

            if (!payout.Withholdings.Any())
                return Failure(MessageKind.Withholding_Cannot_Process_Payout_No_Withholdings);

            Result result = null;
            foreach (var withholding in payout.Withholdings)
            {
                result =
                    await _mediatr.Process(
                        new ProcessWithholdingCommand(request.RequestUser) {WithholdingId = withholding.Id}, token);
                if (!result.Succeeded)
                    break;
            }

            if (result is {Succeeded: false})
                return result;

            return Success();
        }
    }
}
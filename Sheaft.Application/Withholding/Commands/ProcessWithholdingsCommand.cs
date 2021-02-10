using Sheaft.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class ProcessWithholdingsCommand : Command<bool>
    {
        [JsonConstructor]
        public ProcessWithholdingsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
    
    public class ProcessWithholdingsCommandHandler : CommandsHandler,
        IRequestHandler<ProcessWithholdingsCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly PspOptions _pspOptions;

        public ProcessWithholdingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<ProcessWithholdingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<bool>> Handle(ProcessWithholdingsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payout = await _context.GetByIdAsync<Payout>(request.PayoutId, token);
                if (payout.Status != TransactionStatus.Succeeded)
                    return Failed<bool>(new BadRequestException(MessageKind.Withholding_Cannot_Process_Already_Succeeded));

                if(!payout.Withholdings.Any())
                    return Failed<bool>(new BadRequestException(MessageKind.Withholding_Cannot_Process_Payout_No_Withholdings));

                foreach (var withholding in payout.Withholdings)
                {
                    var result = await _mediatr.Process(new ProcessWithholdingCommand(request.RequestUser) { WithholdingId = withholding.Id }, token);
                    if (!result.Success)
                        throw result.Exception;
                }

                return Ok(true);
            });
        }
    }
}

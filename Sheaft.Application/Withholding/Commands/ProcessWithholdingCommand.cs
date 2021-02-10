using Sheaft.Core;
using Newtonsoft.Json;
using System;
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
    public class ProcessWithholdingCommand : Command<TransactionStatus>
    {
        [JsonConstructor]
        public ProcessWithholdingCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid WithholdingId { get; set; }
    }
    
    public class ProcessWithholdingCommandHandler : CommandsHandler,
        IRequestHandler<ProcessWithholdingCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly PspOptions _pspOptions;

        public ProcessWithholdingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<ProcessWithholdingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<TransactionStatus>> Handle(ProcessWithholdingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var withholding = await _context.GetByIdAsync<Withholding>(request.WithholdingId, token);
                if (withholding.Status == TransactionStatus.Succeeded)
                    return Ok(withholding.Status);

                if (withholding.Status != TransactionStatus.Failed && withholding.Status != TransactionStatus.Waiting)
                    return Failed<TransactionStatus>(new BadRequestException(MessageKind.Withholding_Cannot_Process_Pending));

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var result = await _pspService.CreateWithholdingAsync(withholding, token);
                    if (!result.Success)
                        return Failed<TransactionStatus>(result.Exception);

                    withholding.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    withholding.SetIdentifier(result.Data.Identifier);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(withholding.Status);
                }
            });
        }
    }
}

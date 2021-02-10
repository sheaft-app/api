using System;
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
    public class ProcessWithholdingCommand : Command
    {
        [JsonConstructor]
        public ProcessWithholdingCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid WithholdingId { get; set; }
    }

    public class ProcessWithholdingCommandHandler : CommandsHandler,
        IRequestHandler<ProcessWithholdingCommand, Result>
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

        public async Task<Result> Handle(ProcessWithholdingCommand request, CancellationToken token)
        {
            var withholding = await _context.GetByIdAsync<Domain.Withholding>(request.WithholdingId, token);
            if (withholding.Status == TransactionStatus.Succeeded)
                return Success();

            if (withholding.Status != TransactionStatus.Failed && withholding.Status != TransactionStatus.Waiting)
                return Failure(MessageKind.Withholding_Cannot_Process_Pending);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var result = await _pspService.CreateWithholdingAsync(withholding, token);
                if (!result.Succeeded)
                    return Failure(result.Exception);

                withholding.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                withholding.SetIdentifier(result.Data.Identifier);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success();
            }
        }
    }
}
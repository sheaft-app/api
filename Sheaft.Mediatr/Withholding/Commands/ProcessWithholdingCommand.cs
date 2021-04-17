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
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Withholding.Commands
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

        public ProcessWithholdingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<ProcessWithholdingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
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
                    return Failure(result);

                withholding.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                withholding.SetIdentifier(result.Data.Identifier);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success();
            }
        }
    }
}
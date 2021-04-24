using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class RefreshWithholdingStatusCommand : Command
    {
        protected RefreshWithholdingStatusCommand()
        {
            
        }
        [JsonConstructor]
        public RefreshWithholdingStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshWithholdingStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshWithholdingStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshWithholdingStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<RefreshWithholdingStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshWithholdingStatusCommand request,
            CancellationToken token)
        {
            var withholding = await _context.Withholdings
                .SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            
            if (withholding.Processed)
                return Success(withholding.Status);

            var pspResult = await _pspService.GetTransferAsync(withholding.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            withholding.SetStatus(pspResult.Data.Status);
            withholding.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            withholding.SetExecutedOn(pspResult.Data.ProcessedOn);
            withholding.SetAsProcessed();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Declaration.Commands
{
    public class RefreshDeclarationStatusCommand : Command
    {
        [JsonConstructor]
        public RefreshDeclarationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshDeclarationStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshDeclarationStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshDeclarationStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<RefreshDeclarationStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshDeclarationStatusCommand request,
            CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(
                c => c.Declaration.Identifier == request.Identifier, token);
            var pspResult = await _pspService.GetDeclarationAsync(legal.Declaration.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult.Exception);

            legal.Declaration.SetStatus(pspResult.Data.Status);
            legal.Declaration.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            legal.Declaration.SetProcessedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
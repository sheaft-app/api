using System.Linq;
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

namespace Sheaft.Mediatr.Document.Commands
{
    public class RefreshDocumentStatusCommand : Command
    {
        [JsonConstructor]
        public RefreshDocumentStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshDocumentStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshDocumentStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshDocumentStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<RefreshDocumentStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshDocumentStatusCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(
                r => r.Documents.Any(d => d.Identifier == request.Identifier), token);
            var document = legal.Documents.FirstOrDefault(c => c.Identifier == request.Identifier);

            var pspResult = await _pspService.GetDocumentAsync(document.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            document.SetStatus(pspResult.Data.Status);
            document.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            document.SetProcessedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
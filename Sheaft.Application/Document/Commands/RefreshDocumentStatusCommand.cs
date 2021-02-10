using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class RefreshDocumentStatusCommand : Command<DocumentStatus>
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
            IRequestHandler<RefreshDocumentStatusCommand, Result<DocumentStatus>>
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

        public async Task<Result<DocumentStatus>> Handle(RefreshDocumentStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Identifier == request.Identifier), token);
                var document = legal.Documents.FirstOrDefault(c => c.Identifier == request.Identifier);

                var pspResult = await _pspService.GetDocumentAsync(document.Identifier, token);
                if (!pspResult.Success)
                    return Failed<DocumentStatus>(pspResult.Exception);

                document.SetStatus(pspResult.Data.Status);
                document.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                document.SetProcessedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (document.Status)
                {
                    case DocumentStatus.Refused:
                        _mediatr.Post(new DocumentRefusedEvent(request.RequestUser) { DocumentId = document.Id });
                        break;
                    case DocumentStatus.OutOfDate:
                        _mediatr.Post(new DocumentOutdatedEvent(request.RequestUser) { DocumentId = document.Id });
                        break;
                }

                return Ok(document.Status);
            });
        }
    }
}

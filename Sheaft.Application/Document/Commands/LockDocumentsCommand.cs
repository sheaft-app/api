using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class LockDocumentsCommand : Command<bool>
    {
        [JsonConstructor]
        public LockDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
    }
    
    public class LockDocumentsCommandHandler : CommandsHandler,
            IRequestHandler<LockDocumentsCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public LockDocumentsCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<LockDocumentsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(LockDocumentsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Id == request.LegalId, token);
                var success = true;
                foreach (var document in legal.Documents.Where(d => d.Status == DocumentStatus.UnLocked || d.Status == DocumentStatus.Created))
                {
                    var result = await _mediatr.Process(new LockDocumentCommand(request.RequestUser)
                    {
                        DocumentId = document.Id
                    }, token);

                    if (!result.Success)
                        success = false;
                }

                return Ok(success);
            });
        }
    }
}

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
    public class SubmitDocumentsCommand : Command<bool>
    {
        [JsonConstructor]
        public SubmitDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
    }
    
    public class SubmitDocumentsCommandHandler : CommandsHandler,
            IRequestHandler<SubmitDocumentsCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public SubmitDocumentsCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<SubmitDocumentsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }
        
        public async Task<Result<bool>> Handle(SubmitDocumentsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Id == request.LegalId, token);
                var success = true;
                foreach (var document in legal.Documents.Where(d => d.Status == DocumentStatus.Locked))
                {
                    var result = await _mediatr.Process(new SubmitDocumentCommand(request.RequestUser)
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

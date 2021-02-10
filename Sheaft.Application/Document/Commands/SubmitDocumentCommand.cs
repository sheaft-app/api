using System;
using System.Collections.Generic;
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
using Sheaft.Exceptions;

namespace Sheaft.Application.Commands
{
    public class SubmitDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public SubmitDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
    
    public class SubmitDocumentCommandHandler : CommandsHandler,
            IRequestHandler<SubmitDocumentCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public SubmitDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<SubmitDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(SubmitDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
                var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
                if (document.Status != DocumentStatus.Locked)
                    return BadRequest<bool>(MessageKind.Document_CannotSubmit_NotLocked);

                var results = new List<Result<bool>>();
                foreach (var page in document.Pages.Where(p => !p.UploadedOn.HasValue))
                {
                    results.Add(await _mediatr.Process(new SendPageCommand(request.RequestUser)
                    {
                        DocumentId = request.DocumentId,
                        PageId = page.Id
                    }, token));
                }

                if (results.Any(r => !r.Success))
                    return BadRequest<bool>(MessageKind.Document_Errors_On_Submit);

                var result = await _pspService.SubmitDocumentAsync(document, legal.User.Identifier, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                document.SetStatus(result.Data.Status);
                document.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                _context.Update(document);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}

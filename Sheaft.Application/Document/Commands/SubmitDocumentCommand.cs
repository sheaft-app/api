using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Page.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Document.Commands
{
    public class SubmitDocumentCommand : Command
    {
        [JsonConstructor]
        public SubmitDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }

    public class SubmitDocumentCommandHandler : CommandsHandler,
        IRequestHandler<SubmitDocumentCommand, Result>
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

        public async Task<Result> Handle(SubmitDocumentCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<Domain.Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId),
                token);
            var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
            if (document.Status != DocumentStatus.Locked)
                return Failure(MessageKind.Document_CannotSubmit_NotLocked);

            var results = new List<Result>();
            foreach (var page in document.Pages.Where(p => !p.UploadedOn.HasValue))
            {
                results.Add(await _mediatr.Process(new SendPageCommand(request.RequestUser)
                {
                    DocumentId = request.DocumentId,
                    PageId = page.Id
                }, token));
            }

            if (results.Any(r => !r.Succeeded))
                return Failure(MessageKind.Document_Errors_On_Submit);

            var result = await _pspService.SubmitDocumentAsync(document, legal.User.Identifier, token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            document.SetStatus(result.Data.Status);
            document.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

            _context.Update(document);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}
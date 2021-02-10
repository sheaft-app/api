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
using Sheaft.Exceptions;

namespace Sheaft.Application.Commands
{
    public class UpdateDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DocumentKind Kind { get; set; }
    }
    public class UpdateDocumentCommandHandler : CommandsHandler,
            IRequestHandler<UpdateDocumentCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public UpdateDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<UpdateDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(UpdateDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.Id), token);
                var document = legal.Documents.FirstOrDefault(c => c.Id == request.Id);
                if (document.Kind != request.Kind && legal.Documents.Any(d => d.Kind == request.Kind))
                    return BadRequest<bool>(MessageKind.Document_CannotUpdate_Another_Document_With_Type_Exists);

                document.SetKind(request.Kind);
                document.SetName(request.Name);

                if (string.IsNullOrWhiteSpace(document.Identifier))
                {
                    var result = await _pspService.CreateDocumentAsync(document, legal.User.Identifier, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);

                    document.SetIdentifier(result.Data.Identifier);
                    document.SetStatus(result.Data.Status);
                }

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

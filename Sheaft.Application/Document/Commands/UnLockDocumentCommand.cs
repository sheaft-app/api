using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class UnLockDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public UnLockDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
    public class UnLockDocumentCommandHandler : CommandsHandler,
            IRequestHandler<UnLockDocumentCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public UnLockDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<UnLockDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(UnLockDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
                var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
                document.SetStatus(DocumentStatus.UnLocked);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

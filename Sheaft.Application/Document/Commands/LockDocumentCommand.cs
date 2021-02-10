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
    public class LockDocumentCommand : Command<bool>
    {
        [JsonConstructor]
        public LockDocumentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
    
    public class LockDocumentCommandHandler : CommandsHandler,
            IRequestHandler<LockDocumentCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public LockDocumentCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<LockDocumentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(LockDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<Legal>(r => r.Documents.Any(d => d.Id == request.DocumentId), token);
                var document = legal.Documents.FirstOrDefault(c => c.Id == request.DocumentId);
                document.SetStatus(DocumentStatus.Locked);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}

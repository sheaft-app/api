using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class DocumentCommandsHandler : CommandsHandler,
        IRequestHandler<CreateDocumentCommand, Result<Guid>>,
        IRequestHandler<UploadDocumentCommand, Result<bool>>,
        IRequestHandler<UploadPageCommand, Result<bool>>,
        IRequestHandler<SubmitDocumentsCommand, Result<bool>>,
        IRequestHandler<SubmitDocumentCommand, Result<bool>>,
        IRequestHandler<RemoveDocumentCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public DocumentCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<DocumentCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                return Ok(Guid.NewGuid());
            });
        }

        public async Task<Result<bool>> Handle(UploadDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UploadPageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SubmitDocumentsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SubmitDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RemoveDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                return Ok(true);
            });
        }
    }
}

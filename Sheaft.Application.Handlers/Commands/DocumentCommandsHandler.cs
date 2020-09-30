using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Sheaft.Application.Handlers
{
    public class DocumentCommandsHandler : ResultsHandler,
            IRequestHandler<CreateDocumentCommand, Result<Guid>>,
            IRequestHandler<UpdateDocumentCommand, Result<bool>>,
            IRequestHandler<SubmitDocumentsCommand, Result<bool>>,
            IRequestHandler<SubmitDocumentCommand, Result<bool>>,
            IRequestHandler<LockDocumentsCommand, Result<bool>>,
            IRequestHandler<LockDocumentCommand, Result<bool>>,
            IRequestHandler<UnLockDocumentCommand, Result<bool>>,
            IRequestHandler<DeleteDocumentCommand, Result<bool>>,
            IRequestHandler<RefreshDocumentStatusCommand, Result<DocumentStatus>>
    {
        private readonly IPspService _pspService;

        public DocumentCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<DocumentCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    await _context.EnsureNotExists<Document>(d => d.Legal.Id == request.LegalId && d.Kind == request.Kind, token);

                    var legal = await _context.GetSingleAsync<Legal>(r => r.Id == request.LegalId, token);
                    var document = new Document(Guid.NewGuid(), request.Kind, request.Name, legal);

                    await _context.AddAsync(document, token);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateDocumentAsync(document, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    document.SetIdentifier(result.Data.Identifier);
                    document.SetStatus(result.Data.Status);

                    _context.Update(document);
                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(document.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.Id, token);

                if (document.Kind != request.Kind)
                {
                    await _context.EnsureNotExists<Document>(d => d.Legal.Id == document.Legal.Id && d.Kind == request.Kind, token);
                    document.SetKind(request.Kind);
                }

                document.SetName(request.Name);

                if (string.IsNullOrWhiteSpace(document.Identifier))
                {
                    var result = await _pspService.CreateDocumentAsync(document, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);

                    document.SetIdentifier(result.Data.Identifier);
                    document.SetStatus(result.Data.Status);
                }

                _context.Update(document);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(LockDocumentsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var documents = await _context.GetAsync<Document>(c => c.Legal.User.Id == request.RequestUser.Id && c.Status == DocumentStatus.Created, token);
                var success = true;
                foreach (var document in documents)
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

        public async Task<Result<bool>> Handle(LockDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.DocumentId, token);
                if (document.Status != DocumentStatus.Created || document.Status != DocumentStatus.UnLocked)
                    return Failed<bool>(new InvalidOperationException());

                document.SetStatus(DocumentStatus.Locked);

                _context.Update(document);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UnLockDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.DocumentId, token);
                document.SetStatus(DocumentStatus.UnLocked);

                _context.Update(document);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SubmitDocumentsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var documents = await _context.GetAsync<Document>(c => c.Legal.User.Id == request.RequestUser.Id && c.Status == DocumentStatus.Locked, token);
                var success = true;
                foreach (var document in documents)
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

        public async Task<Result<bool>> Handle(SubmitDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.DocumentId, token);
                if (document.Status != DocumentStatus.Locked)
                    return Failed<bool>(new InvalidOperationException());

                var results = new List<Result<bool>>();
                foreach(var page in document.Pages.Where(p => !p.UploadedOn.HasValue))
                {
                    results.Add(await _mediatr.Process(new SendPageCommand(request.RequestUser)
                    {
                        DocumentId = request.DocumentId,
                        PageId = page.Id
                    }, token));
                }

                if (results.Any(r => !r.Success))
                    return Failed<bool>(new InvalidOperationException());

                var result = await _pspService.SubmitDocumentAsync(document, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                document.SetStatus(result.Data.Status);
                document.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.Id, token);
                _context.Remove(document);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<DocumentStatus>> Handle(RefreshDocumentStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetSingleAsync<Document>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetDocumentAsync(document.Identifier, token);
                if (!pspResult.Success)
                    return Failed<DocumentStatus>(pspResult.Exception);

                document.SetStatus(pspResult.Data.Status);
                document.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                document.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(document);
                await _context.SaveChangesAsync(token);

                switch (document.Status)
                {
                    case DocumentStatus.Refused:
                        await _mediatr.Post(new DocumentRefusedEvent(request.RequestUser) { DocumentId = document.Id }, token);
                        break;
                    case DocumentStatus.OutOfDate:
                        await _mediatr.Post(new DocumentOutdatedEvent(request.RequestUser) { DocumentId = document.Id }, token);
                        break;
                    case DocumentStatus.Validated:
                        await _mediatr.Post(new DocumentValidatedEvent(request.RequestUser) { DocumentId = document.Id }, token);
                        break;
                }

                return Ok(document.Status);
            });
        }
    }
}

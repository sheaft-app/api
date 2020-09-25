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

namespace Sheaft.Application.Handlers
{
    public class DocumentCommandsHandler : ResultsHandler,
            IRequestHandler<CreateDocumentCommand, Result<Guid>>,
            IRequestHandler<UploadDocumentCommand, Result<bool>>,
            IRequestHandler<UploadPageCommand, Result<bool>>,
            IRequestHandler<SubmitDocumentsCommand, Result<bool>>,
            IRequestHandler<SubmitDocumentCommand, Result<bool>>,
            IRequestHandler<RemoveDocumentCommand, Result<bool>>,
            IRequestHandler<RefreshDocumentStatusCommand, Result<bool>>
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
                    var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                    var document = new Document(Guid.NewGuid(), request.Kind, request.Name, user);

                    await _context.AddAsync(document, token);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateDocumentAsync(document, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    document.SetIdentifier(result.Data.Identifier);
                    document.SetValidationStatus(result.Data.Status);

                    _context.Update(document);
                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(document.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UploadDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var success = true;
                foreach (var page in request.Pages)
                {
                    var result = await _mediatr.Process(page, token);
                    if (!result.Success)
                        success = false;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(UploadPageCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.DocumentId, token);

                var page = new Page(Guid.NewGuid(), request.FileName, request.Extension, request.Size);

                document.AddPage(page);
                _context.Update(document);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.AddPageToDocumentAsync(page, document, request.Data, token);
                if (result.Success)
                {
                    page.SetUploaded();
                    _context.Update(page);
                    await _context.SaveChangesAsync(token);
                }

                return result;
            });
        }

        public async Task<Result<bool>> Handle(SubmitDocumentsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var documents = await _context.GetAsync<Document>(c => c.User.Id == request.RequestUser.Id, token);
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

                var result = await _pspService.SubmitDocumentAsync(document, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                document.SetValidationStatus(result.Data.Status);
                document.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RemoveDocumentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetByIdAsync<Document>(request.Id, token);
                _context.Remove(document);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(RefreshDocumentStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var document = await _context.GetSingleAsync<Document>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetDocumentAsync(document.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                document.SetValidationStatus(pspResult.Data.Status);
                document.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                document.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(document);
                var success = await _context.SaveChangesAsync(token) > 0;

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

                return Ok(success);
            });
        }
    }
}

using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using Sheaft.Exceptions;

namespace Sheaft.Application.Handlers
{
    public class DeclarationCommandsHandler : ResultsHandler,
           IRequestHandler<CreateDeclarationCommand, Result<Guid>>,
           IRequestHandler<SubmitDeclarationCommand, Result<bool>>,
           IRequestHandler<LockDeclarationCommand, Result<bool>>,
           IRequestHandler<UnLockDeclarationCommand, Result<bool>>,
           IRequestHandler<RefreshDeclarationStatusCommand, Result<DeclarationStatus>>
    {
        private readonly IPspService _pspService;

        public DeclarationCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<DeclarationCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateDeclarationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetByIdAsync<BusinessLegal>(request.LegalId, token);
                legal.SetDeclaration();

                var result = await _pspService.CreateUboDeclarationAsync(legal.Declaration, legal.User, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                legal.Declaration.SetIdentifier(result.Data.Identifier);
                legal.Declaration.SetStatus(result.Data.Status);
                legal.Declaration.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                await _context.SaveChangesAsync(token);

                return Ok(legal.Declaration.Id);
            });
        }

        public async Task<Result<bool>> Handle(SubmitDeclarationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(r => r.Declaration.Id == request.DeclarationId, token);
                if (legal.Declaration.Status != DeclarationStatus.Locked)
                    return BadRequest<bool>(MessageKind.Declaration_CannotSubmit_NotLocked);

                if (string.IsNullOrWhiteSpace(legal.Declaration.Identifier))
                {
                    var createResult = await _mediatr.Process(new CreateDeclarationCommand(request.RequestUser) { LegalId = legal.Id }, token);
                    if (!createResult.Success)
                        return Failed<bool>(createResult.Exception);
                }

                var submitResult = await _pspService.SubmitUboDeclarationAsync(legal.Declaration, legal.User, token);
                if (!submitResult.Success)
                    return Failed<bool>(submitResult.Exception);

                legal.Declaration.SetStatus(submitResult.Data.Status);
                legal.Declaration.SetResult(submitResult.Data.ResultCode, submitResult.Data.ResultMessage);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(LockDeclarationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(r => r.Declaration.Id == request.DeclarationId, token);
                legal.Declaration.SetStatus(DeclarationStatus.Locked);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UnLockDeclarationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(r => r.Declaration.Id == request.DeclarationId, token);
                legal.Declaration.SetStatus(DeclarationStatus.UnLocked);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<DeclarationStatus>> Handle(RefreshDeclarationStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.Declaration.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetDeclarationAsync(legal.Declaration.Identifier, token);
                if (!pspResult.Success)
                    return Failed<DeclarationStatus>(pspResult.Exception);

                legal.Declaration.SetStatus(pspResult.Data.Status);
                legal.Declaration.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                legal.Declaration.SetProcessedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (legal.Declaration.Status)
                {
                    case DeclarationStatus.Incomplete:
                        _mediatr.Post(new DeclarationIncompleteEvent(request.RequestUser) { DeclarationId = legal.Declaration.Id });
                        break;
                    case DeclarationStatus.Refused:
                        _mediatr.Post(new DeclarationRefusedEvent(request.RequestUser) { DeclarationId = legal.Declaration.Id });
                        break;
                    case DeclarationStatus.Validated:
                        _mediatr.Post(new DeclarationValidatedEvent(request.RequestUser) { DeclarationId = legal.Declaration.Id });
                        break;
                }

                return Ok(legal.Declaration.Status);
            });
        }
    }
}
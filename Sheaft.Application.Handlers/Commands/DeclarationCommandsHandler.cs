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

namespace Sheaft.Application.Handlers
{
    public class DeclarationCommandsHandler : ResultsHandler,
           IRequestHandler<CreateDeclarationCommand, Result<Guid>>,
           IRequestHandler<SubmitDeclarationCommand, Result<bool>>,
           IRequestHandler<RefreshDeclarationStatusCommand, Result<bool>>,
           IRequestHandler<CheckDeclarationConfigurationCommand, Result<bool>>
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
            return await ExecuteAsync(async () =>
            {
                var uboDeclaration = new UboDeclaration(Guid.NewGuid());
                await _context.AddAsync(uboDeclaration);

                var legal = await _context.GetByIdAsync<BusinessLegal>(request.LegalId, token);
                legal.SetUboDeclaration(uboDeclaration);

                _context.Update(legal);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateUboDeclarationAsync(uboDeclaration, legal.Business, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                uboDeclaration.SetIdentifier(result.Data.Identifier);
                uboDeclaration.SetStatus(result.Data.Status);
                uboDeclaration.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                _context.Update(uboDeclaration);
                await _context.SaveChangesAsync(token);

                return Ok(uboDeclaration.Id);
            });
        }

        public async Task<Result<bool>> Handle(SubmitDeclarationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetByIdAsync<BusinessLegal>(request.LegalId, token);
                var result = await _pspService.SubmitUboDeclarationAsync(legal.UboDeclaration, legal.Business, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                legal.UboDeclaration.SetStatus(result.Data.Status);
                legal.UboDeclaration.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                _context.Update(legal);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(RefreshDeclarationStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var declaration = await _context.GetSingleAsync<UboDeclaration>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetDeclarationAsync(declaration.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                declaration.SetStatus(pspResult.Data.Status);
                declaration.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                declaration.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(declaration);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (declaration.Status)
                {
                    case DeclarationStatus.Incomplete:
                        await _mediatr.Post(new DeclarationIncompleteEvent(request.RequestUser) { DeclarationId = declaration.Id }, token);
                        break;
                    case DeclarationStatus.Refused:
                        await _mediatr.Post(new DeclarationRefusedEvent(request.RequestUser) { DeclarationId = declaration.Id }, token);
                        break;
                    case DeclarationStatus.Validated:
                        await _mediatr.Post(new DeclarationValidatedEvent(request.RequestUser) { DeclarationId = declaration.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(CheckDeclarationConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(bl => bl.Business.Id == request.UserId, token);
                if (legal.UboDeclaration == null)
                {
                    var result = await _mediatr.Process(new CreateDeclarationCommand(request.RequestUser)
                    {
                        LegalId = legal.Id
                    }, token);

                    if (!result.Success)
                        return Failed<bool>(result.Exception);
                }
                else if (string.IsNullOrWhiteSpace(legal.UboDeclaration.Identifier))
                {
                    var result = await _pspService.CreateUboDeclarationAsync(legal.UboDeclaration, legal.Business, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);

                    legal.UboDeclaration.SetIdentifier(result.Data.Identifier);
                    legal.UboDeclaration.SetStatus(result.Data.Status);
                    legal.UboDeclaration.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

                    _context.Update(legal.UboDeclaration);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }
    }
}
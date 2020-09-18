using Sheaft.Application.Commands;
using Sheaft.Infrastructure.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using Sheaft.Services.Interop;
using System.Linq;
using Sheaft.Interop.Enums;
using Sheaft.Application.Events;

namespace Sheaft.Application.Handlers
{
    public class DeclarationCommandsHandler : ResultsHandler,
           IRequestHandler<CreateDeclarationCommand, Result<Guid>>,
           IRequestHandler<SubmitDeclarationCommand, Result<bool>>,
           IRequestHandler<SetDeclarationStatusCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IMediator _mediatr;

        public DeclarationCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IMediator mediatr,
            ILogger<DeclarationCommandsHandler> logger) : base(logger)
        {
            _mediatr = mediatr;
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateDeclarationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var uboDeclaration = new UboDeclaration(Guid.NewGuid());
                await _context.AddAsync(uboDeclaration);
                await _context.SaveChangesAsync(token);

                var legal = await _context.GetByIdAsync<BusinessLegal>(request.LegalId, token);
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

        public async Task<Result<bool>> Handle(SetDeclarationStatusCommand request, CancellationToken token)
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

                switch (request.Kind)
                {
                    case PspEventKind.UBO_DECLARATION_INCOMPLETE:
                        await _mediatr.Publish(new DeclarationIncompleteEvent(request.RequestUser) { DeclarationId = declaration.Id }, token);
                        break;
                    case PspEventKind.UBO_DECLARATION_REFUSED:
                        await _mediatr.Publish(new DeclarationRefusedEvent(request.RequestUser) { DeclarationId = declaration.Id }, token);
                        break;
                    case PspEventKind.UBO_DECLARATION_VALIDATED:
                        await _mediatr.Publish(new DeclarationValidatedEvent(request.RequestUser) { DeclarationId = declaration.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }
    }
}
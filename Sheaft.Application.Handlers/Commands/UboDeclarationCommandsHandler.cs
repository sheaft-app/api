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

namespace Sheaft.Application.Handlers
{
    public class UboDeclarationCommandsHandler : ResultsHandler,
           IRequestHandler<CreateUboDeclarationCommand, Result<Guid>>,
           IRequestHandler<SubmitUboDeclarationCommand, Result<bool>>,
           IRequestHandler<SetUboDeclarationValidatedCommand, Result<bool>>,
           IRequestHandler<SetUboDeclarationValidationCommand, Result<bool>>,
           IRequestHandler<SetUboDeclarationIncompleteCommand, Result<bool>>,
           IRequestHandler<SetUboDeclarationRefusedCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public UboDeclarationCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<UboDeclarationCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateUboDeclarationCommand request, CancellationToken token)
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

        public async Task<Result<bool>> Handle(SubmitUboDeclarationCommand request, CancellationToken token)
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

        public Task<Result<bool>> Handle(SetUboDeclarationValidatedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetUboDeclarationValidationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetUboDeclarationIncompleteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetUboDeclarationRefusedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
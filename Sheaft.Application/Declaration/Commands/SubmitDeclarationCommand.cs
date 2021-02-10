using System;
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
    public class SubmitDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public SubmitDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid DeclarationId { get; set; }
    }
    
    public class SubmitDeclarationCommandHandler : CommandsHandler,
           IRequestHandler<SubmitDeclarationCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public SubmitDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<SubmitDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
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
    }
}

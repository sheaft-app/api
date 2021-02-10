using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class RefreshDeclarationStatusCommand : Command<DeclarationStatus>
    {
        [JsonConstructor]
        public RefreshDeclarationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
    
    public class RefreshDeclarationStatusCommandHandler : CommandsHandler,
           IRequestHandler<RefreshDeclarationStatusCommand, Result<DeclarationStatus>>
    {
        private readonly IPspService _pspService;

        public RefreshDeclarationStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<RefreshDeclarationStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
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

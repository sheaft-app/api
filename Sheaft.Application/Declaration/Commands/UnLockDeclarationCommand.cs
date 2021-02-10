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

namespace Sheaft.Application.Commands
{
    public class UnLockDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public UnLockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid DeclarationId { get; set; }
    }
    
    public class UnLockDeclarationCommandHandler : CommandsHandler,
           IRequestHandler<UnLockDeclarationCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public UnLockDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<UnLockDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
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
    }
}

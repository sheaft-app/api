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
    public class LockDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public LockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid DeclarationId { get; set; }
    }
    
    public class LockDeclarationCommandHandler : CommandsHandler,
           IRequestHandler<LockDeclarationCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public LockDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<LockDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
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
    }
}

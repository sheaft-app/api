using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Declaration.Commands
{
    public class UnLockDeclarationCommand : Command
    {
        [JsonConstructor]
        public UnLockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }

    public class UnLockDeclarationCommandHandler : CommandsHandler,
        IRequestHandler<UnLockDeclarationCommand, Result>
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

        public async Task<Result> Handle(UnLockDeclarationCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(r => r.Declaration.Id == request.DeclarationId,
                token);
            legal.Declaration.SetStatus(DeclarationStatus.UnLocked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
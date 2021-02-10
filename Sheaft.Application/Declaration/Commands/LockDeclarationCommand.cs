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
    public class LockDeclarationCommand : Command
    {
        [JsonConstructor]
        public LockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }

    public class LockDeclarationCommandHandler : CommandsHandler,
        IRequestHandler<LockDeclarationCommand, Result>
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

        public async Task<Result> Handle(LockDeclarationCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(r => r.Declaration.Id == request.DeclarationId,
                token);
            legal.Declaration.SetStatus(DeclarationStatus.Locked);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
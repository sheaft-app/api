using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Declaration.Commands
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
        public UnLockDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UnLockDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
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
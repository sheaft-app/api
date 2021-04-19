using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Declaration.Commands
{
    public class CreateDeclarationCommand : Command<Guid>
    {
        protected CreateDeclarationCommand()
        {
            
        }
        [JsonConstructor]
        public CreateDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }
    }

    public class CreateDeclarationCommandHandler : CommandsHandler,
        IRequestHandler<CreateDeclarationCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateDeclarationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreateDeclarationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateDeclarationCommand request, CancellationToken token)
        {
            var legal = await _context.Set<BusinessLegal>().SingleAsync(e => e.Id == request.LegalId, token);
            legal.SetDeclaration();

            var result = await _pspService.CreateUboDeclarationAsync(legal.Declaration, legal.User, token);
            if (!result.Succeeded)
                return Failure<Guid>(result);

            legal.Declaration.SetIdentifier(result.Data.Identifier);
            legal.Declaration.SetStatus(result.Data.Status);
            legal.Declaration.SetResult(result.Data.ResultCode, result.Data.ResultMessage);

            await _context.SaveChangesAsync(token);
            return Success(legal.Declaration.Id);
        }
    }
}
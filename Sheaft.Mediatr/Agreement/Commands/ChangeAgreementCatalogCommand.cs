using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class ChangeAgreementCatalogCommand : Command
    {
        protected ChangeAgreementCatalogCommand()
        {
        }

        [JsonConstructor]
        public ChangeAgreementCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public Guid CatalogId { get; set; }
    }

    public class ChangeAgreementCatalogCommandHandler : CommandsHandler,
        IRequestHandler<ChangeAgreementCatalogCommand, Result>
    {
        private readonly RoleOptions _roleOptions;
        
        public ChangeAgreementCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ChangeAgreementCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(ChangeAgreementCatalogCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(e => e.Id == request.AgreementId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            var catalog = await _context.Catalogs.SingleAsync(c => c.Id == request.CatalogId, token);
            entity.ChangeCatalog(catalog);
            
            //TODO update quickorder products

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class AssignCatalogToAgreementCommand : Command
    {
        [JsonConstructor]
        public AssignCatalogToAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public Guid CatalogId { get; set; }
    }

    public class AssignCatalogToAgreementCommandHandler : CommandsHandler,
        IRequestHandler<AssignCatalogToAgreementCommand, Result>
    {
        public AssignCatalogToAgreementCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AssignCatalogToAgreementCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AssignCatalogToAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(entity.CreatedBy.Kind == ProfileKind.Store && entity.Delivery.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if(entity.CreatedBy.Kind == ProfileKind.Producer && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            var catalog = await _context.GetSingleAsync<Domain.Catalog>(c => c.IsDefault && c.Kind == CatalogKind.Stores && c.Producer.Id == entity.Delivery.Producer.Id, token);
            entity.AssignCatalog(catalog);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
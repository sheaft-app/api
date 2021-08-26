using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class DeleteCatalogCommand : Command
    {
        protected DeleteCatalogCommand()
        {
            
        }
        public DeleteCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid CatalogId { get; set; }
    }

    public class DeleteCatalogCommandHandler : CommandsHandler,
        IRequestHandler<DeleteCatalogCommand, Result>
    {
        public DeleteCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteCatalogCommand request, CancellationToken token)
        {
            var entity =
                await _context.Catalogs
                    .SingleOrDefaultAsync(c => c.Id == request.CatalogId, token);

            if (entity.Kind == CatalogKind.Consumers)
                return Failure("Impossible de supprimer le catalogue pour les particuliers, désactivez-le.");

            var agreements = await _context.Agreements
                .Where(a => a.Catalog != null && a.CatalogId == entity.Id)
                .ToListAsync(token);

            if (agreements.Any(a => a.Status == AgreementStatus.Accepted))
                return Failure("Impossible de supprimer ce catalogue, certains partenariat y sont rattaché.");

            foreach (var catalogProduct in entity.Products.ToList())
                catalogProduct.Product.RemoveFromCatalog(entity.Id);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
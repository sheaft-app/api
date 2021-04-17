using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class DeleteCatalogCommand: Command
    {
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
            var catalogs =
                await _context.GetAsync<Domain.Catalog>(
                    c => c.Producer.Id == request.RequestUser.Id, token);
            
            var entity = catalogs.Single(c => c.Id == request.CatalogId);
            if(entity.IsDefault)
                throw SheaftException.Validation();

            var agreements = await _context.Agreements
                .Where(a => a.Catalog != null && a.Catalog.Id == entity.Id)
                .ToListAsync(token);
            
            if(agreements.Any(a => a.Status == AgreementStatus.Accepted))
                throw SheaftException.Validation();

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}
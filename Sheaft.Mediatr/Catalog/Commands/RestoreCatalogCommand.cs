using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class RestoreCatalogCommand : Command
    {
        protected RestoreCatalogCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid CatalogId { get; set; }
    }

    public class RestoreCatalogCommandHandler : CommandsHandler,
        IRequestHandler<RestoreCatalogCommand, Result>
    {
        public RestoreCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreCatalogCommand request, CancellationToken token)
        {
            var entity =
                await _context.Catalogs.SingleOrDefaultAsync(a => a.Id == request.CatalogId && a.RemovedOn.HasValue, token);
            
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}
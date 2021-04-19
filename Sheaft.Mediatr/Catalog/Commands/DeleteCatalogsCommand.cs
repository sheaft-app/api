using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class DeleteCatalogsCommand: Command
    {
        protected DeleteCatalogsCommand()
        {
            
        }
        public DeleteCatalogsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> CatalogIds { get; set; }
    }

    public class DeleteCatalogsCommandHandler : CommandsHandler,
        IRequestHandler<DeleteCatalogsCommand, Result>
    {
        public DeleteCatalogsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteCatalogsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteCatalogsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var catalogId in request.CatalogIds)
                {
                    var result =
                        await _mediatr.Process(new DeleteCatalogCommand(request.RequestUser) {CatalogId = catalogId},
                            token);

                    if (!result.Succeeded)
                        throw result.Exception;
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
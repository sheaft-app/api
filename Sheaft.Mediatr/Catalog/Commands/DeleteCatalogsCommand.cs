using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog
{
    public class DeleteCatalogsCommand: Command
    {
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
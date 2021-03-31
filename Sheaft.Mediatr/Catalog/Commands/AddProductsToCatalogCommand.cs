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

namespace Sheaft.Mediatr.Catalog
{
    public class AddProductsToCatalogCommand: Command
    {
        public AddProductsToCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
    }

    public class AddProductsToCatalogCommandHandler : CommandsHandler,
        IRequestHandler<AddProductsToCatalogCommand, Result>
    {
        public AddProductsToCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AddProductsToCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public Task<Result> Handle(AddProductsToCatalogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
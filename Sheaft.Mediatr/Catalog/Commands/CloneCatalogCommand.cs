using System;
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
    public class CloneCatalogCommand: Command<Guid>
    {
        public CloneCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public decimal Percent { get; set; }
    }

    public class CloneCatalogCommandHandler : CommandsHandler,
        IRequestHandler<CloneCatalogCommand, Result<Guid>>
    {
        public CloneCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CloneCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public Task<Result<Guid>> Handle(CloneCatalogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
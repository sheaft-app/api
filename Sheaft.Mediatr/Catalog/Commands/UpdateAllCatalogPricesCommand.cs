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
    public class UpdateAllCatalogPricesCommand: Command
    {
        public UpdateAllCatalogPricesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public decimal Percent { get; set; }
    }

    public class UpdateAllCatalogPricesCommandHandler : CommandsHandler,
        IRequestHandler<UpdateAllCatalogPricesCommand, Result>
    {
        public UpdateAllCatalogPricesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateAllCatalogPricesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public Task<Result> Handle(UpdateAllCatalogPricesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
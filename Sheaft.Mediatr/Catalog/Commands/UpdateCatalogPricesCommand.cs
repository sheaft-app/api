using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Catalog
{
    public class UpdateCatalogPricesCommand: Command
    {
        public UpdateCatalogPricesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public IEnumerable<UpdateOrCreateCatalogPriceDto> Prices { get; set; }
    }

    public class UpdateCatalogPricesCommandHandler : CommandsHandler,
        IRequestHandler<UpdateCatalogPricesCommand, Result>
    {
        public UpdateCatalogPricesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateCatalogPricesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public Task<Result> Handle(UpdateCatalogPricesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
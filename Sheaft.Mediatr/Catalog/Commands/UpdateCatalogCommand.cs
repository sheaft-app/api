using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Catalog.Commands
{
    public class UpdateCatalogCommand: Command
    {
        protected UpdateCatalogCommand()
        {
            
        }
        public UpdateCatalogCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<ProductPriceInputDto> Products { get; set; }
    }

    public class UpdateCatalogCommandHandler : CommandsHandler,
        IRequestHandler<UpdateCatalogCommand, Result>
    {
        public UpdateCatalogCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateCatalogCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateCatalogCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var catalogs =
                    await _context.Catalogs
                        .Where(c => c.ProducerId == request.RequestUser.Id)
                        .ToListAsync(token);

                var entity = catalogs.Single(c => c.Id == request.CatalogId);
                if (!request.IsDefault && entity.Kind == CatalogKind.Consumers)
                    return Failure("Impossible de désassigner ce catalogue particulier comme catalogue par défaut.");

                entity.SetIsAvailable(request.IsAvailable);
                entity.SetName(request.Name);

                if (entity.Kind != CatalogKind.Consumers)
                {
                    entity.SetIsDefault(request.IsDefault);

                    if (entity.IsDefault &&
                        catalogs.Any(c => c.Id != entity.Id && c.IsDefault && c.Kind == CatalogKind.Stores))
                    {
                        var actualDefaultCatalog =
                            catalogs.Single(c => c.Id != entity.Id && c.IsDefault && c.Kind == CatalogKind.Stores);

                        actualDefaultCatalog.SetIsDefault(false);
                    }

                    if (catalogs.Where(c => c.Kind == CatalogKind.Stores).All(c => !c.IsDefault))
                        return Failure("Au moins un catalogue professionel doit être défini par défaut.");
                }

                var existingProductIds = entity.Products.Select(c => c.ProductId).ToList();
                var productIdsToRemove = existingProductIds
                    .Except(request.Products?.Select(c => c.ProductId) ?? new List<Guid>()).ToList();
                if (productIdsToRemove.Any())
                {
                    var removeResult =
                        await _mediatr.Process(
                            new RemoveProductsFromCatalogCommand(request.RequestUser)
                                {CatalogId = request.CatalogId, ProductIds = productIdsToRemove}, token);

                    if (!removeResult.Succeeded)
                        return removeResult;
                }

                var result =
                    await _mediatr.Process(
                        new AddOrUpdateProductsToCatalogCommand(request.RequestUser)
                            {CatalogId = request.CatalogId, Products = request.Products}, token);

                if (!result.Succeeded)
                    return result;

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}
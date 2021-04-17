using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Product.Commands
{
    public class DeleteProductCommand : Command
    {
        [JsonConstructor]
        public DeleteProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : CommandsHandler,
        IRequestHandler<DeleteProductCommand, Result>
    {
        public DeleteProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            if (entity.CatalogsPrices != null && entity.CatalogsPrices.Any())
                foreach (var catalogProduct in entity.CatalogsPrices.ToList())
                    entity.RemoveFromCatalog(catalogProduct.Catalog.Id);
            
            _context.Remove(entity);
            await _context.SaveChangesAsync(token);
            
            _mediatr.Post(new UpdateProducerProductsCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
            return Success();
        }
    }
}
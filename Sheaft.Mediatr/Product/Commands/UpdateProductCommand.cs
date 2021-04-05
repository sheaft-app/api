using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Product.Commands
{
    public class UpdateProductCommand : Command
    {
        [JsonConstructor]
        public UpdateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string OriginalPicture { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
        public Guid? ReturnableId { get; set; }
        public bool? VisibleToStores { get; set; }
        public bool? VisibleToConsumers { get; set; }
        public decimal? WholeSalePricePerUnit { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<UpdateOrCreateCatalogPriceDto> Catalogs { get; set; }
    }

    public class UpdateProductCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IIdentifierService _identifierService;

        public UpdateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            ILogger<UpdateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
                if (entity.Producer.Id != request.RequestUser.Id)
                    throw SheaftException.Forbidden();

                var reference = request.Reference;
                if (!string.IsNullOrWhiteSpace(reference) && reference != entity.Reference)
                {
                    var existingEntity = await _context.FindSingleAsync<Domain.Product>(
                        p => p.Reference == reference && p.Producer.Id == entity.Producer.Id, token);
                    if (existingEntity != null)
                        return Failure(MessageKind.CreateProduct_Reference_AlreadyExists, reference);
                }

                if (string.IsNullOrWhiteSpace(reference))
                {
                    var resultIdentifier =
                        await _identifierService.GetNextProductReferenceAsync(entity.Producer.Id, token);
                    if (!resultIdentifier.Succeeded)
                        return Failure(resultIdentifier.Exception);

                    reference = resultIdentifier.Data;
                }

                entity.SetVat(request.Vat);
                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetReference(reference);
                entity.SetWeight(request.Weight);
                entity.SetAvailable(request.Available);
                entity.SetConditioning(request.Conditioning, request.QuantityPerUnit, request.Unit);

                if (request.ReturnableId.HasValue)
                {
                    var returnable = await _context.GetByIdAsync<Domain.Returnable>(request.ReturnableId.Value, token);
                    entity.SetReturnable(returnable);
                }
                else
                {
                    entity.SetReturnable(null);
                }

                var tags = await _context.FindAsync<Domain.Tag>(t => request.Tags.Contains(t.Id), token);
                entity.SetTags(tags);

                if (request.VisibleToConsumers.HasValue && request.VisibleToStores.HasValue)
                {
                    var consumerCatalog = await _context.GetSingleAsync<Domain.Catalog>(
                        c => c.Producer.Id == entity.Producer.Id && c.Kind == CatalogKind.Consumers, token);

                    if (request.VisibleToConsumers.Value)
                        consumerCatalog.AddOrUpdateProduct(entity, request.WholeSalePricePerUnit.Value);
                    else if(consumerCatalog.Products.Any(pc => pc.Product.Id == entity.Id))
                        consumerCatalog.RemoveProduct(entity.Id); 

                    var storeCatalog = await _context.GetSingleAsync<Domain.Catalog>(
                        c => c.Producer.Id == entity.Producer.Id && c.Kind == CatalogKind.Stores, token);
                    
                    if (request.VisibleToStores.Value)
                        storeCatalog.AddOrUpdateProduct(entity, request.WholeSalePricePerUnit.Value);
                    else if(storeCatalog.Products.Any(pc => pc.Product.Id == entity.Id))
                        storeCatalog.RemoveProduct(entity.Id);
                }

                if (!request.VisibleToConsumers.HasValue || !request.VisibleToStores.HasValue)
                {
                    var productCatalogs = entity.CatalogsPrices.Select(p => p.Catalog);
                    var catalogIds = productCatalogs.Select(pc => pc.Id);
                    var catalogToRemoveIds = catalogIds.Except(request.Catalogs.Select(c => c.Id));
                    foreach (var catalog in productCatalogs.Where(pc => catalogToRemoveIds.Contains(pc.Id)))
                        catalog.RemoveProduct(entity.Id);

                    foreach (var catalogPrice in request.Catalogs)
                    {
                        var catalog = entity.CatalogsPrices.FirstOrDefault(c => c.Catalog.Id == catalogPrice.Id)?.Catalog ?? await _context.GetByIdAsync<Domain.Catalog>(catalogPrice.Id, token);
                        catalog.AddOrUpdateProduct(entity, request.WholeSalePricePerUnit.Value);
                    }
                }

                await _context.SaveChangesAsync(token);

                var imageResult = await _mediatr.Process(
                    new UpdateProductPreviewCommand(request.RequestUser)
                    {
                        ProductId = entity.Id,
                        Picture = new PictureSourceDto {Resized = request.Picture, Original = request.OriginalPicture}
                    }, token);

                if (!imageResult.Succeeded)
                    return Failure(imageResult.Exception);

                await transaction.CommitAsync(token);

                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
                return Success();
            }
        }
    }
}

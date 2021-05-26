using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
        protected UpdateProductCommand()
        {
        }

        [JsonConstructor]
        public UpdateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
        public Guid? ReturnableId { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<PictureInputDto> Pictures { get; set; }
        public IEnumerable<CatalogPriceInputDto> Catalogs { get; set; }
    }

    public class UpdateProductCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IPictureService _imageService;
        private readonly IIdentifierService _identifierService;

        public UpdateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            IIdentifierService identifierService,
            ILogger<UpdateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
            _identifierService = identifierService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = await _context.Products.SingleAsync(p => p.Id == request.ProductId, token);
                if (entity.ProducerId != request.RequestUser.Id)
                    return Failure(MessageKind.Forbidden);

                var reference = request.Reference;
                if (!string.IsNullOrWhiteSpace(reference) && reference != entity.Reference)
                {
                    var existingEntity = await _context.Products.AnyAsync(
                        p => p.Reference == reference && p.ProducerId == entity.ProducerId, token);
                    if (existingEntity)
                        return Failure(MessageKind.CreateProduct_Reference_AlreadyExists, reference);
                }

                if (string.IsNullOrWhiteSpace(reference))
                {
                    var resultIdentifier =
                        await _identifierService.GetNextProductReferenceAsync(entity.ProducerId, token);
                    if (!resultIdentifier.Succeeded)
                        return Failure(resultIdentifier);

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
                    var returnable =
                        await _context.Returnables.SingleAsync(e => e.Id == request.ReturnableId.Value, token);
                    entity.SetReturnable(returnable);
                }
                else
                {
                    entity.SetReturnable(null);
                }

                var tags = await _context.Tags.Where(t => request.Tags.Contains(t.Id)).ToListAsync(token);
                entity.SetTags(tags);

                var productCatalogs = entity.CatalogsPrices.Select(p => p.Catalog);
                var catalogIds = productCatalogs.Select(pc => pc.Id);
                var catalogToRemoveIds = catalogIds.Except(request.Catalogs.Select(c => c.CatalogId));

                foreach (var catalog in productCatalogs.Where(pc => catalogToRemoveIds.Contains(pc.Id)))
                    entity.RemoveFromCatalog(catalog.Id);

                foreach (var catalogPrice in request.Catalogs)
                {
                    var catalog = entity.CatalogsPrices.FirstOrDefault(c => c.CatalogId == catalogPrice.CatalogId)
                                      ?.Catalog ??
                                  await _context.Catalogs.SingleAsync(e => e.Id == catalogPrice.CatalogId, token);
                    entity.AddOrUpdateCatalogPrice(catalog, catalogPrice.WholeSalePricePerUnit);
                }
                
                if (request.Pictures != null && request.Pictures.Any())
                {
                    var pictures = entity.Pictures.ToList();
                    entity.ClearPictures();
                    
                    var result = Success<string>();
                    foreach (var picture in request.Pictures.OrderBy(p => p.Position))
                    {
                        var existingPicture =
                            picture.Id.HasValue ? pictures.SingleOrDefault(c => c.Id == picture.Id.Value) : null;

                        if (existingPicture != null)
                        {
                            existingPicture.SetPosition(picture.Position);
                            entity.AddPicture(existingPicture);
                        }
                        else
                        {
                            var id = Guid.NewGuid();
                            result = await _imageService.HandleProductPictureAsync(entity, id, picture.Data, token);
                            if (!result.Succeeded)
                                break;

                            entity.AddPicture(new ProductPicture(id, result.Data, picture.Position));
                        }
                    }

                    if (!result.Succeeded)
                        return Failure(result);
                }
                
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) {ProducerId = entity.ProducerId});
                return Success();
            }
        }
    }
}
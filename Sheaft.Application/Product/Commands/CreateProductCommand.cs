using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Picture.Commands;
using Sheaft.Application.Producer.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Product.Commands
{
    public class CreateProductCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string OriginalPicture { get; set; }
        public decimal WholeSalePricePerUnit { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public UnitKind Unit { get; set; }
        public ConditioningKind Conditioning { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
        public bool? VisibleToConsumers { get; set; }
        public bool? VisibleToStores { get; set; }
        public Guid? ReturnableId { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public bool SkipUpdateProducerTags { get; set; } = false;
    }

    public class CreateProductCommandHandler : CommandsHandler,
        IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IIdentifierService _identifierService;

        public CreateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            ILogger<CreateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken token)
        {
            var reference = request.Reference;
            if (!string.IsNullOrWhiteSpace(reference))
            {
                var existingEntity =
                    await _context.FindSingleAsync<Domain.Product>(
                        p => p.Reference == reference && p.Producer.Id == request.ProducerId, token);
                if (existingEntity != null)
                    return Failure<Guid>(MessageKind.CreateProduct_Reference_AlreadyExists, reference);
            }
            else
            {
                var resultIdentifier =
                    await _identifierService.GetNextProductReferenceAsync(request.ProducerId, token);
                if (!resultIdentifier.Succeeded)
                    return Failure<Guid>(resultIdentifier.Exception);

                reference = resultIdentifier.Data;
            }

            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = new Domain.Product(Guid.NewGuid(), reference, request.Name, request.WholeSalePricePerUnit,
                    request.Conditioning, request.Unit, request.QuantityPerUnit, producer);

                entity.SetVat(request.Vat);
                entity.SetDescription(request.Description);
                entity.SetAvailable(request.Available ?? true);
                entity.SetStoreVisibility(request.VisibleToStores ?? false);
                entity.SetConsumerVisibility(request.VisibleToConsumers ?? false);
                entity.SetWeight(request.Weight);

                if (request.ReturnableId.HasValue)
                {
                    var returnable = await _context.GetByIdAsync<Domain.Returnable>(request.ReturnableId.Value, token);
                    entity.SetReturnable(returnable);
                }

                var tags = await _context.FindAsync<Domain.Tag>(t => request.Tags.Contains(t.Id), token);
                entity.SetTags(tags);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                var imageResult = await _mediatr.Process(
                    new UpdateProductPictureCommand(request.RequestUser)
                    {
                        ProductId = entity.Id, Picture = request.Picture, OriginalPicture = request.OriginalPicture
                    }, token);
                if (!imageResult.Succeeded)
                    return Failure<Guid>(imageResult.Exception);

                await transaction.CommitAsync(token);

                if (!request.SkipUpdateProducerTags)
                    _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser)
                        {ProducerId = request.ProducerId});

                return Success(entity.Id);
            }
        }
    }
}
﻿using System;
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
    public class UpdateProductCommand : Command
    {
        [JsonConstructor]
        public UpdateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
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
    }

    public class UpdateProductCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly IIdentifierService _identifierService;

        public UpdateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IIdentifierService identifierService,
            ILogger<UpdateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _identifierService = identifierService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = await _context.GetByIdAsync<Domain.Product>(request.Id, token);

                var reference = request.Reference;
                if (!string.IsNullOrWhiteSpace(reference) && reference != entity.Reference)
                {
                    var existingEntity = await _context.FindSingleAsync<Domain.Product>(
                        p => p.Reference == reference && p.Producer.Id == request.RequestUser.Id, token);
                    if (existingEntity != null)
                        return Failure(MessageKind.CreateProduct_Reference_AlreadyExists, reference);
                }

                if (string.IsNullOrWhiteSpace(reference))
                {
                    var resultIdentifier =
                        await _identifierService.GetNextProductReferenceAsync(request.RequestUser.Id, token);
                    if (!resultIdentifier.Succeeded)
                        return Failure(resultIdentifier.Exception);

                    reference = resultIdentifier.Data;
                }

                entity.SetVat(request.Vat);
                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetWholeSalePricePerUnit(request.WholeSalePricePerUnit);
                entity.SetReference(reference);
                entity.SetWeight(request.Weight);
                entity.SetAvailable(request.Available);
                entity.SetStoreVisibility(request.VisibleToStores);
                entity.SetConsumerVisibility(request.VisibleToConsumers);
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

                await _context.SaveChangesAsync(token);

                var imageResult = await _mediatr.Process(
                    new UpdateProductPictureCommand(request.RequestUser)
                    {
                        ProductId = entity.Id, Picture = request.Picture, OriginalPicture = request.OriginalPicture
                    }, token);
                if (!imageResult.Succeeded)
                    return Failure(imageResult.Exception);

                await transaction.CommitAsync(token);

                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) {ProducerId = request.RequestUser.Id});
                return Success();
            }
        }
    }
}
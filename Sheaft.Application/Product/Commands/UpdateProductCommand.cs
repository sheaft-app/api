using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;

namespace Sheaft.Application.Commands
{
    public class UpdateProductCommand : ProductCommand<bool>
    {
        [JsonConstructor]
        public UpdateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class UpdateProductCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                    var reference = request.Reference;
                    if (!string.IsNullOrWhiteSpace(reference) && reference != entity.Reference)
                    {
                        var existingEntity = await _context.FindSingleAsync<Product>(p => p.Reference == reference && p.Producer.Id == request.RequestUser.Id, token);
                        if (existingEntity != null)
                            return ValidationError<bool>(MessageKind.CreateProduct_Reference_AlreadyExists, reference);
                    }

                    if (string.IsNullOrWhiteSpace(reference))
                    {
                        var resultIdentifier = await _identifierService.GetNextProductReferenceAsync(request.RequestUser.Id, token);
                        if (!resultIdentifier.Success)
                            return Failed<bool>(resultIdentifier.Exception);

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
                        var returnable = await _context.GetByIdAsync<Returnable>(request.ReturnableId.Value, token);
                        entity.SetReturnable(returnable);
                    }
                    else
                    {
                        entity.SetReturnable(null);
                    }

                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    entity.SetTags(tags);

                    await _context.SaveChangesAsync(token);

                    var imageResult = await _mediatr.Process(new UpdateProductPictureCommand(request.RequestUser) { ProductId = entity.Id, Picture = request.Picture, OriginalPicture = request.OriginalPicture }, token);
                    if (!imageResult.Success)
                        return Failed<bool>(imageResult.Exception);

                    await transaction.CommitAsync(token);

                    _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) { ProducerId = request.RequestUser.Id });
                    return Ok(true);
                }
            });
        }
    }
}

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
    public class CreateProductCommand : ProductCommand<Guid>
    {
        [JsonConstructor]
        public CreateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public bool SkipUpdateProducerTags { get; set; } = false;
    }
    
    public class CreateProductCommandHandler : CommandsHandler,
        IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IBlobService _blobService;
        private readonly IIdentifierService _identifierService;

        public CreateProductCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IIdentifierService identifierService,
            ILogger<CreateProductCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _identifierService = identifierService;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var reference = request.Reference;
                    if (!string.IsNullOrWhiteSpace(reference))
                    {
                        var existingEntity = await _context.FindSingleAsync<Product>(p => p.Reference == reference && p.Producer.Id == request.RequestUser.Id, token);
                        if (existingEntity != null)
                            return ValidationError<Guid>(MessageKind.CreateProduct_Reference_AlreadyExists, reference);
                    }
                    else
                    {
                        var resultIdentifier = await _identifierService.GetNextProductReferenceAsync(request.RequestUser.Id, token);
                        if (!resultIdentifier.Success)
                            return Failed<Guid>(resultIdentifier.Exception);

                        reference = resultIdentifier.Data;
                    }

                    var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                    var entity = new Product(Guid.NewGuid(), reference, request.Name, request.WholeSalePricePerUnit, request.Conditioning, request.Unit, request.QuantityPerUnit, producer);

                    entity.SetVat(request.Vat);
                    entity.SetDescription(request.Description);
                    entity.SetAvailable(request.Available ?? true);
                    entity.SetStoreVisibility(request.VisibleToStores ?? false);
                    entity.SetConsumerVisibility(request.VisibleToConsumers ?? false);
                    entity.SetWeight(request.Weight);

                    if (request.ReturnableId.HasValue)
                    {
                        var returnable = await _context.GetByIdAsync<Returnable>(request.ReturnableId.Value, token);
                        entity.SetReturnable(returnable);
                    }

                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    entity.SetTags(tags);

                    await _context.AddAsync(entity, token);
                    await _context.SaveChangesAsync(token);

                    var imageResult = await _mediatr.Process(new UpdateProductPictureCommand(request.RequestUser) { ProductId = entity.Id, Picture = request.Picture, OriginalPicture = request.OriginalPicture }, token);
                    if (!imageResult.Success)
                        return Failed<Guid>(imageResult.Exception);

                    await transaction.CommitAsync(token);

                    if (!request.SkipUpdateProducerTags)
                        _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) { ProducerId = request.RequestUser.Id });

                    return Created(entity.Id);
                }
            });
        }
    }
}

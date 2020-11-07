using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class ProductCommandsHandler : ResultsHandler,
        IRequestHandler<CreateProductCommand, Result<Guid>>,
        IRequestHandler<UpdateProductCommand, Result<bool>>,
        IRequestHandler<SetProductsAvailabilityCommand, Result<bool>>,
        IRequestHandler<SetProductAvailabilityCommand, Result<bool>>,
        IRequestHandler<SetProductsSearchabilityCommand, Result<bool>>,
        IRequestHandler<SetProductSearchabilityCommand, Result<bool>>,
        IRequestHandler<RateProductCommand, Result<bool>>,
        IRequestHandler<DeleteProductsCommand, Result<bool>>,
        IRequestHandler<DeleteProductCommand, Result<bool>>,
        IRequestHandler<QueueImportProductsCommand, Result<Guid>>,
        IRequestHandler<ImportProductsCommand, Result<bool>>,
        IRequestHandler<RestoreProductCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public ProductCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ProductCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                if (!string.IsNullOrWhiteSpace(request.Reference))
                {
                    var existingEntity = await _context.FindSingleAsync<Product>(p => p.Reference == request.Reference && p.Producer.Id == request.RequestUser.Id, token);
                    if (existingEntity != null)
                        return ValidationError<Guid>(MessageKind.CreateProduct_Reference_AlreadyExists, request.Reference);
                }
                else
                {
                    var resultIdentifier = await _mediatr.Process(new CreateProductIdentifierCommand(request.RequestUser) { ProducerId = request.RequestUser.Id }, token);
                    if (!resultIdentifier.Success)
                        return Failed<Guid>(resultIdentifier.Exception);

                    request.Reference = resultIdentifier.Data;
                }

                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var entity = new Product(Guid.NewGuid(), request.Reference, request.Name, request.WholeSalePricePerUnit, request.Conditioning, request.Unit, request.QuantityPerUnit, request.Vat, producer);

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

                if (!request.SkipUpdateProducerTags)
                    _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) { ProducerId = request.RequestUser.Id });

                var imageResult = await _mediatr.Process(new UpdateProductPictureCommand(request.RequestUser) { ProductId = entity.Id, Picture = request.Picture }, token);
                if (!imageResult.Success)
                    return Failed<Guid>(imageResult.Exception);

                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetVat(request.Vat);
                entity.SetWholeSalePricePerUnit(request.WholeSalePricePerUnit);
                entity.SetReference(request.Reference);
                entity.SetWeight(request.Weight);
                entity.SetAvailable(request.Available);
                entity.SetStoreVisibility(request.VisibleToStores);
                entity.SetConsumerVisibility(request.VisibleToConsumers);
                entity.SetConditioning(request.Conditioning, request.QuantityPerUnit, request.Unit);
                entity.SetWeight(request.Weight);

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

                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) { ProducerId = request.RequestUser.Id });

                var imageResult = await _mediatr.Process(new UpdateProductPictureCommand(request.RequestUser) { ProductId = entity.Id, Picture = request.Picture }, token);
                if (!imageResult.Success)
                    return Failed<bool>(imageResult.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                entity.AddRating(user, request.Value, request.Comment);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.RateProduct, UserId = request.RequestUser.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SetProductsAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new SetProductAvailabilityCommand(request.RequestUser) { Id = id, Available = request.Available }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(SetProductAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                entity.SetAvailable(request.Available);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SetProductsSearchabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new SetProductSearchabilityCommand(request.RequestUser) { Id = id, VisibleToStores = request.VisibleToStores, VisibleToConsumers = request.VisibleToConsumers }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(SetProductSearchabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                entity.SetConsumerVisibility(request.VisibleToConsumers);
                entity.SetStoreVisibility(request.VisibleToStores);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new DeleteProductCommand(request.RequestUser) { Id = id }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(QueueImportProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var entity = new Job(Guid.NewGuid(), JobKind.ImportProducts, $"Import produits", producer);

                var response = await _blobService.UploadImportProductsFileAsync(producer.Id, entity.Id, request.FileStream, token);
                if (!response.Success)
                    return Failed<Guid>(response.Exception);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new ImportProductsCommand(request.RequestUser) { Id = entity.Id, NotifyOnUpdates = request.NotifyOnUpdates });
                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(ImportProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var products = new List<Product>();
                var job = await _context.GetByIdAsync<Job>(request.Id, token);

                try
                {
                    var startResult = await _mediatr.Process(new StartJobCommand(request.RequestUser) { Id = job.Id }, token);
                    if (!startResult.Success)
                        throw startResult.Exception;

                    if(request.NotifyOnUpdates)
                        _mediatr.Post(new ProductImportProcessingEvent(request.RequestUser) { JobId = job.Id });

                    var data = await _blobService.DownloadImportProductsFileAsync(job.User.Id, job.Id, token);
                    if (!data.Success)
                        throw data.Exception;

                    using (var stream = new MemoryStream(data.Data))
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            if (worksheet == null)
                                throw new BadRequestException(MessageKind.ImportProduct_Missing_Tab);

                            using (var transaction = await _context.BeginTransactionAsync(token))
                            {
                                for (var i = 2; i <= worksheet.Dimension.Rows; i++)
                                {
                                    var command = await CreateProductCommandFromRowDatasAsync(worksheet, request, i, token);
                                    if (!command.Success)
                                        throw command.Exception;

                                    var productResult = await _mediatr.Process(command.Data, token);
                                    if (!productResult.Success)
                                        throw productResult.Exception;
                                }

                                await _context.SaveChangesAsync(token);
                                await transaction.CommitAsync(token);

                                _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser) { ProducerId = request.RequestUser.Id });
                            }
                        }
                    }

                    var result = await _mediatr.Process(new CompleteJobCommand(request.RequestUser) { Id = job.Id }, token);
                    if (!result.Success)
                        throw result.Exception;

                    if (request.NotifyOnUpdates)
                        _mediatr.Post(new ProductImportSucceededEvent(request.RequestUser) { JobId = job.Id });

                    return result;
                }
                catch (Exception e)
                {
                    if (request.NotifyOnUpdates)
                        _mediatr.Post(new ProductImportFailedEvent(request.RequestUser) { JobId = job.Id });

                    return await _mediatr.Process(new FailJobCommand(request.RequestUser) { Id = job.Id, Reason = e.Message }, token);
                }
            });
        }

        public async Task<Result<bool>> Handle(RestoreProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Products.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        #region Product implementation

        private async Task<Result<CreateProductCommand>> CreateProductCommandFromRowDatasAsync(ExcelWorksheet worksheet,
            ImportProductsCommand request, int i, CancellationToken token)
        {
            var nameStr = worksheet.Cells[i, 2].GetValue<string>();
            if (string.IsNullOrWhiteSpace(nameStr))
                return Failed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_Name_Required_Line, i));

            var createProductCommand = new CreateProductCommand(request.RequestUser)
            {
                Reference = worksheet.Cells[i, 1].GetValue<string>(),
                Name = nameStr,
                Description = worksheet.Cells[i, 10].GetValue<string>()
            };

            var wholeSalePriceStr = worksheet.Cells[i, 3].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".").Replace("€", "");
            var vatStr = worksheet.Cells[i, 4].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".").Replace("%", "").Replace("en", "");
            var conditioningStr = worksheet.Cells[i, 5].GetValue<string>()?.ToLowerInvariant().Replace("\"", "").Replace("'", "").Replace(".", ",").Split(",").Select(t => t.Trim()).FirstOrDefault();
            var quantityPerUnitStr = worksheet.Cells[i, 6].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".");
            var unitKindStr = worksheet.Cells[i, 7].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".").Split(",").Select(t => t.Trim()).FirstOrDefault();
            var tagsStr = worksheet.Cells[i, 8].GetValue<string>()?.Replace("\"", "").Replace("'", "").Replace(".", ",").Split(",").Select(t => t.Trim()).FirstOrDefault();
            var bioStr = worksheet.Cells[i, 9].GetValue<string>()?.ToLowerInvariant().Replace(" ", "");

            if (!decimal.TryParse(wholeSalePriceStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal wholeSalePrice))
                return Failed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_WholeSalePrice_Invalid_Line, i));
            else
                createProductCommand.WholeSalePricePerUnit = wholeSalePrice;

            if (!decimal.TryParse(vatStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal vat) || (vat != 5.5m && vat != 10m && vat != 20m))
                return Failed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_Vat_Invalid_Line, i));
            else
                createProductCommand.Vat = vat;

            if (string.IsNullOrWhiteSpace(quantityPerUnitStr))
                quantityPerUnitStr = "1";

            if (!decimal.TryParse(quantityPerUnitStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal qtyPerUnit))
                return Failed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_QtyPerUnit_Invalid_Line, i));
            else
                createProductCommand.QuantityPerUnit = qtyPerUnit;

            switch (conditioningStr)
            {
                case "poids":
                    createProductCommand.Conditioning = ConditioningKind.Bulk;
                    break;
                case "bouquet":
                    createProductCommand.Conditioning = ConditioningKind.Bouquet;
                    break;
                case "boîte":
                    createProductCommand.Conditioning = ConditioningKind.Box;
                    break;
                case "botte":
                    createProductCommand.Conditioning = ConditioningKind.Bunch;
                    break;
                case "pièce":
                    createProductCommand.Conditioning = ConditioningKind.Piece;
                    break;
                case "panier garni":
                    createProductCommand.Conditioning = ConditioningKind.Basket;
                    break;
                default:
                    createProductCommand.Conditioning = ConditioningKind.Not_Specified;
                    break;
            }

            if (createProductCommand.Conditioning == ConditioningKind.Bulk)
            {
                if (!Enum.TryParse(unitKindStr, true, out UnitKind unitKind))
                    return Failed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_UnitKind_Invalid_Line, i));
                else
                    createProductCommand.Unit = unitKind;
            }

            if(tagsStr == "Panier garni")
            {
                createProductCommand.Conditioning = ConditioningKind.Basket;
            }

            switch (bioStr)
            {
                case "oui":
                    tagsStr += ";bio";
                    break;
            }

            var tagsAsStr = tagsStr.Split(";").ToList();

            var tags = await _context.FindAsync<Tag>(t => tagsAsStr.Contains(t.Name), token);
            createProductCommand.Tags = tags.Select(t => t.Id);

            createProductCommand.Available = false;
            createProductCommand.VisibleToConsumers = false;
            createProductCommand.VisibleToStores = false;
            createProductCommand.SkipUpdateProducerTags = true;

            return Ok(createProductCommand);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Core.Models;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Options;
using Sheaft.Services.Interop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Sheaft.Application.Handlers
{
    public class ProductCommandsHandler : CommandsHandler,
        IRequestHandler<CreateProductCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateProductCommand, CommandResult<bool>>,
        IRequestHandler<SetProductAvailabilityCommand, CommandResult<bool>>,
        IRequestHandler<SetProductsAvailabilityCommand, CommandResult<bool>>,
        IRequestHandler<RateProductCommand, CommandResult<bool>>,
        IRequestHandler<DeleteProductsCommand, CommandResult<bool>>,
        IRequestHandler<DeleteProductCommand, CommandResult<bool>>,
        IRequestHandler<QueueImportProductsCommand, CommandResult<Guid>>,
        IRequestHandler<ImportProductsCommand, CommandResult<bool>>,
        IRequestHandler<UpdateProductPictureCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediatr;
        private readonly IIdentifierService _identifierService;
        private readonly IQueueService _queuesService;
        private readonly IBlobService _blobsService;
        private readonly StorageOptions _storageOptions;
        private readonly HttpClient _httpClient;

        public ProductCommandsHandler(
            IOptionsSnapshot<StorageOptions> storageOptions,
            IMediator mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            IQueueService queuesService,
            IBlobService blobsService,
            IHttpClientFactory httpFactory,
            ILogger<ProductCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _mediatr = mediatr;
            _identifierService = identifierService;
            _queuesService = queuesService;
            _blobsService = blobsService;
            _httpClient = httpFactory.CreateClient("products");

            _storageOptions = storageOptions.Value;
        }

        public async Task<CommandResult<Guid>> Handle(CreateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var company = await _context.GetByIdAsync<Company>(request.RequestUser.CompanyId, token);

                if (!string.IsNullOrWhiteSpace(request.Reference))
                {
                    var existingEntity = await _context.FindSingleAsync<Product>(p => p.Reference == request.Reference && p.Producer.Id == company.Id, token);
                    if (existingEntity != null)
                        return ValidationResult<Guid>(new KeyValuePair<MessageKind, object[]>(MessageKind.CreateProduct_Reference_AlreadyExists, new List<object> { request.Reference }.ToArray()));
                }
                else
                {
                    var result = await _identifierService.GetNextProductReferenceAsync(request.RequestUser.CompanyId, token);
                    if (!result.Success)
                        return CommandFailed<Guid>(result.Exception);

                    request.Reference = result.Result;
                }

                var entity = new Product(Guid.NewGuid(), request.Reference, request.Name, request.WholeSalePricePerUnit, request.Unit, request.QuantityPerUnit, request.Vat, company);

                entity.SetWeight(request.Weight);
                entity.SetDescription(request.Description);
                entity.SetAvailable(request.Available);

                if (request.PackagingId.HasValue)
                {
                    var packaging = await _context.GetByIdAsync<Packaging>(request.PackagingId.Value, token);
                    entity.SetPackaging(packaging);
                }

                await HandleImageAsync(entity, request.Picture, token);

                var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                entity.SetTags(tags);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return CreatedResult(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetVat(request.Vat);
                entity.SetWholeSalePricePerUnit(request.WholeSalePricePerUnit);
                entity.SetReference(request.Reference);
                entity.SetWeight(request.Weight);
                entity.SetAvailable(request.Available);
                entity.SetUnit(request.QuantityPerUnit, request.Unit);

                if (request.PackagingId.HasValue)
                {
                    var packaging = await _context.GetByIdAsync<Packaging>(request.PackagingId.Value, token);
                    entity.SetPackaging(packaging);
                }
                else
                {
                    entity.SetPackaging(null);
                }

                await HandleImageAsync(entity, request.Picture, token);

                var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                entity.SetTags(tags);

                var entry = _context.Update(entity);
                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RateProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                entity.AddRating(user, request.Value, request.Comment);
                var result = await _context.SaveChangesAsync(token);

                await _queuesService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.RateProduct, UserId = request.RequestUser.Id }, token);

                return OkResult(result > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(SetProductsAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Send(new SetProductAvailabilityCommand(request.RequestUser) { Id = id, Available = request.Available });
                        if (!result.Success)
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(SetProductAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);
                entity.SetAvailable(request.Available);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Send(new DeleteProductCommand(request.RequestUser) { Id = id });
                        if (!result.Success)
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteProductCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.Id, token);

                entity.SetReference(DateTimeOffset.UtcNow.ToString("_" + entity.Reference));
                _context.Products.Remove(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<Guid>> Handle(QueueImportProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var company = await _context.GetByIdAsync<Company>(request.RequestUser.CompanyId, token);

                var entity = new Job(Guid.NewGuid(), JobKind.ImportProducts, $"Import produits", user, ImportProductsCommand.QUEUE_NAME);

                var response = await _blobsService.UploadImportProductsFileAsync(company.Id, entity.Id, request.FileName, request.FileStream, token);
                if (!response.Success)
                    return CommandFailed<Guid>(response.Exception);

                entity.SetCommand(new ImportProductsCommand(request.RequestUser) { Id = entity.Id, Uri = response.Result });

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                await _queuesService.InsertJobToProcessAsync(entity, token);

                return OkResult(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(ImportProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var products = new List<Product>();
                var job = await _context.GetByIdAsync<Job>(request.Id, token);

                try
                {
                    var startResult = await _mediatr.Send(new StartJobCommand(request.RequestUser) { Id = job.Id });
                    if (!startResult.Success)
                        throw startResult.Exception;

                    await _queuesService.ProcessEventAsync(ProductImportProcessingEvent.QUEUE_NAME, new ProductImportProcessingEvent(request.RequestUser) { Id = job.Id }, token);

                    using (var stream = new MemoryStream())
                    {
                        var data = await _blobsService.DownloadImportProductsFileAsync(request.Uri, token);
                        if (!data.Success)
                            throw data.Exception;

                        await data.Result.CopyToAsync(stream, token);
                        stream.Position = 0;

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            if (worksheet == null)
                                throw new BadRequestException(MessageKind.ImportProduct_Missing_Tab);

                            using (var transaction = await _context.Database.BeginTransactionAsync(token))
                            {
                                for (var i = 2; i <= worksheet.Dimension.Rows; i++)
                                {
                                    var command = await CreateProductCommandFromRowDatasAsync(worksheet, request, i, token);
                                    if (!command.Success)
                                        throw command.Exception;

                                    var productResult = await _mediatr.Send(command.Result, token);
                                    if (!productResult.Success)
                                        throw productResult.Exception;
                                }

                                await _context.SaveChangesAsync(token);
                                await transaction.CommitAsync(token);
                            }
                        }
                    }

                    var result = await _mediatr.Send(new CompleteJobCommand(request.RequestUser) { Id = job.Id }, token);
                    if (!result.Success)
                        throw result.Exception;

                    await _queuesService.ProcessEventAsync(ProductImportSucceededEvent.QUEUE_NAME, new ProductImportSucceededEvent(request.RequestUser) { Id = job.Id }, token);
                    return result;
                }
                catch (Exception e)
                {
                    await _queuesService.ProcessEventAsync(ProductImportFailedEvent.QUEUE_NAME, new ProductImportFailedEvent(request.RequestUser) { Id = job.Id }, token);
                    return await _mediatr.Send(new FailJobCommand(request.RequestUser) { Id = job.Id, Reason = e.Message }, token);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateProductPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var product = await _context.GetByIdAsync<Product>(request.Id, token);

                await HandleImageAsync(product, request.Picture, token);
                var entry = _context.Update(product);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        #region Product implementation

        private async Task HandleImageAsync(Product product, string picture, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(picture))
            {
                product.SetImage(null);
                return;
            }

            byte[] bytes = null;
            if (!picture.StartsWith("http") && !picture.StartsWith("https"))
            {
                var base64Data = picture.StartsWith("data:image") ? Regex.Match(picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value : picture;
                bytes = Convert.FromBase64String(base64Data);
            }
            else if(!picture.StartsWith("https://blob.") && !picture.Contains("core.windows.net"))
            {
                using (var response = await _httpClient.GetAsync(picture))
                    bytes = await response.Content.ReadAsByteArrayAsync();
            }

            var imageId = Guid.NewGuid().ToString("N");

            using (Image image = Image.Load(bytes))
            {
                await UploadImageAsync(image, product.Producer.Id, product.Id, imageId, ImageSize.LARGE, 620, 256, token);
                await UploadImageAsync(image, product.Producer.Id, product.Id, imageId, ImageSize.MEDIUM, 310, 128, token);
                await UploadImageAsync(image, product.Producer.Id, product.Id, imageId, ImageSize.SMALL, 64, 64, token, ResizeMode.Crop);
            }

            product.SetImage($"https://{_storageOptions.Account}.blob.{_storageOptions.Suffix}/{_storageOptions.Containers.Pictures}/{CoreProductExtensions.GetImageUrl(product.Producer.Id, product.Id, imageId)}");
        }

        private async Task UploadImageAsync(Image image, Guid companyId, Guid productId, string filename, string size, int width, int height, CancellationToken token, ResizeMode mode = ResizeMode.Max, int quality = 100)
        {
            using (var blobStream = new MemoryStream())
            {
                image.Clone(context => context.Resize(new ResizeOptions
                {
                    Mode = mode,
                    Size = new Size(width, height)
                })).Save(blobStream, new JpegEncoder { Quality = quality });

                await _blobsService.UploadProductPictureAsync(companyId, productId, filename, size, blobStream, token);
            }
        }

        private async Task<CommandResult<CreateProductCommand>> CreateProductCommandFromRowDatasAsync(ExcelWorksheet worksheet,
            ImportProductsCommand request, int i, CancellationToken token)
        {
            var nameStr = worksheet.Cells[i, 2].GetValue<string>();
            if (string.IsNullOrWhiteSpace(nameStr))
                return CommandFailed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_Name_Required_Line, i));

            var createProductCommand = new CreateProductCommand(request.RequestUser)
            {
                Reference = worksheet.Cells[i, 1].GetValue<string>(),
                Name = nameStr,
                Description = worksheet.Cells[i, 3].GetValue<string>()
            };

            var wholeSalePriceStr = worksheet.Cells[i, 4].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".").Replace("€", "");
            var vatStr = worksheet.Cells[i, 5].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".").Replace("%", "").Replace("en", "");
            var weightStr = worksheet.Cells[i, 6].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".").Replace("gr", "").Replace("grammes", "").Replace("gramme", "");
            var availableValueStr = worksheet.Cells[i, 7].GetValue<string>()?.ToLowerInvariant().Replace(" ", "");
            var tagsStr = worksheet.Cells[i, 8].GetValue<string>()?.ToLowerInvariant().Replace("\"", "").Replace("'", "").Replace(".", ",").Split(",").Select(t => t.Trim());

            if (!decimal.TryParse(wholeSalePriceStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal wholeSalePrice))
                return CommandFailed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_WholeSalePrice_Invalid_Line, i));
            else
                createProductCommand.WholeSalePricePerUnit = wholeSalePrice;

            if (!decimal.TryParse(vatStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal vat))
                return CommandFailed<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_Vat_Invalid_Line, i));
            else
                createProductCommand.Vat = vat;

            if (!string.IsNullOrWhiteSpace(weightStr) && decimal.TryParse(weightStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal weight))
                createProductCommand.Weight = weight;

            switch (availableValueStr)
            {
                case "non":
                case "no":
                case "ko":
                case "0":
                    createProductCommand.Available = false;
                    break;
                default:
                    createProductCommand.Available = true;
                    break;
            }

            var tags = await _context.FindAsync<Tag>(t => tagsStr.Contains(t.Name), token);
            createProductCommand.Tags = tags.Select(t => t.Id);

            return OkResult(createProductCommand);
        }

        #endregion
    }
}

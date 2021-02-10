using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Job.Commands;
using Sheaft.Application.Producer.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Product;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Product.Commands
{
    public class ImportProductsCommand : Command
    {
        [JsonConstructor]
        public ImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool NotifyOnUpdates { get; set; } = true;
    }

    public class ImportProductsCommandHandler : CommandsHandler,
        IRequestHandler<ImportProductsCommand, Result>
    {
        private readonly IBlobService _blobService;

        public ImportProductsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ImportProductsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(ImportProductsCommand request, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Domain.Job>(request.Id, token);

            try
            {
                var startResult = await _mediatr.Process(new StartJobCommand(request.RequestUser) {Id = job.Id}, token);
                if (!startResult.Succeeded)
                    throw startResult.Exception;

                if (request.NotifyOnUpdates)
                    _mediatr.Post(new ProductImportProcessingEvent(job.Id));

                var data = await _blobService.DownloadImportProductsFileAsync(job.User.Id, job.Id, token);
                if (!data.Succeeded)
                    throw data.Exception;

                using (var stream = new MemoryStream(data.Data))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                            throw SheaftException.BadRequest(MessageKind.ImportProduct_Missing_Tab);

                        using (var transaction = await _context.BeginTransactionAsync(token))
                        {
                            for (var i = 2; i <= worksheet.Dimension.Rows; i++)
                            {
                                var command = await CreateProductCommandFromRowDatasAsync(worksheet, request, i, token);
                                if (!command.Succeeded)
                                    throw command.Exception;

                                var productResult = await _mediatr.Process(command.Data, token);
                                if (!productResult.Succeeded)
                                    throw productResult.Exception;
                            }

                            await _context.SaveChangesAsync(token);
                            await transaction.CommitAsync(token);

                            _mediatr.Post(new UpdateProducerTagsCommand(request.RequestUser)
                                {ProducerId = request.RequestUser.Id});
                        }
                    }
                }

                var result = await _mediatr.Process(new CompleteJobCommand(request.RequestUser) {Id = job.Id}, token);
                if (!result.Succeeded)
                    throw result.Exception;

                if (request.NotifyOnUpdates)
                    _mediatr.Post(new ProductImportSucceededEvent(job.Id));

                return result;
            }
            catch (Exception e)
            {
                if (request.NotifyOnUpdates)
                    _mediatr.Post(new ProductImportFailedEvent(job.Id));

                return await _mediatr.Process(new FailJobCommand(request.RequestUser) {Id = job.Id, Reason = e.Message},
                    token);
            }
        }

        private async Task<Result<CreateProductCommand>> CreateProductCommandFromRowDatasAsync(ExcelWorksheet worksheet,
            ImportProductsCommand request, int i, CancellationToken token)
        {
            var nameStr = worksheet.Cells[i, 2].GetValue<string>();
            if (string.IsNullOrWhiteSpace(nameStr))
                return Failure<CreateProductCommand>(
                    new ValidationException(MessageKind.CreateProduct_Name_Required_Line, i));

            var createProductCommand = new CreateProductCommand(request.RequestUser)
            {
                Reference = worksheet.Cells[i, 1].GetValue<string>(),
                Name = nameStr,
                Description = worksheet.Cells[i, 10].GetValue<string>()
            };

            var wholeSalePriceStr = worksheet.Cells[i, 3].GetValue<string>()?.ToLowerInvariant().Replace(" ", "")
                .Replace(",", ".").Replace("€", "");
            var vatStr = worksheet.Cells[i, 4].GetValue<string>()?.ToLowerInvariant().Replace(" ", "").Replace(",", ".")
                .Replace("%", "").Replace("en", "");
            var conditioningStr = worksheet.Cells[i, 5].GetValue<string>()?.ToLowerInvariant().Replace("\"", "")
                .Replace("'", "").Replace(".", ",").Split(",").Select(t => t.Trim()).FirstOrDefault();
            var quantityPerUnitStr = worksheet.Cells[i, 6].GetValue<string>()?.ToLowerInvariant().Replace(" ", "")
                .Replace(",", ".");
            var unitKindStr = worksheet.Cells[i, 7].GetValue<string>()?.ToLowerInvariant().Replace(" ", "")
                .Replace(",", ".").Split(",").Select(t => t.Trim()).FirstOrDefault();
            var tagsStr = worksheet.Cells[i, 8].GetValue<string>()?.Replace("\"", "").Replace("'", "").Replace(".", ",")
                .Split(",").Select(t => t.Trim()).FirstOrDefault();
            var bioStr = worksheet.Cells[i, 9].GetValue<string>()?.ToLowerInvariant().Replace(" ", "");

            if (!decimal.TryParse(wholeSalePriceStr, NumberStyles.Any, new CultureInfo("en-US"),
                out decimal wholeSalePrice))
                return Failure<CreateProductCommand>(
                    new ValidationException(MessageKind.CreateProduct_WholeSalePrice_Invalid_Line, i));
            else
                createProductCommand.WholeSalePricePerUnit = wholeSalePrice;

            if (!decimal.TryParse(vatStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal vat) ||
                (vat != 5.5m && vat != 10m && vat != 20m))
                return Failure<CreateProductCommand>(new ValidationException(MessageKind.CreateProduct_Vat_Invalid_Line,
                    i));
            else
                createProductCommand.Vat = vat;

            if (string.IsNullOrWhiteSpace(quantityPerUnitStr))
                quantityPerUnitStr = "1";

            if (!decimal.TryParse(quantityPerUnitStr, NumberStyles.Any, new CultureInfo("en-US"),
                out decimal qtyPerUnit))
                return Failure<CreateProductCommand>(
                    new ValidationException(MessageKind.CreateProduct_QtyPerUnit_Invalid_Line, i));
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
                    return Failure<CreateProductCommand>(
                        new ValidationException(MessageKind.CreateProduct_UnitKind_Invalid_Line, i));
                else
                    createProductCommand.Unit = unitKind;
            }

            if (tagsStr == "Panier garni")
            {
                createProductCommand.Conditioning = ConditioningKind.Basket;
            }

            switch (bioStr)
            {
                case "oui":
                    tagsStr += ";Bio";
                    break;
            }

            var tagsAsStr = tagsStr.Split(";").ToList();

            var tags = await _context.FindAsync<Domain.Tag>(t => tagsAsStr.Contains(t.Name), token);
            createProductCommand.Tags = tags.Select(t => t.Id);

            createProductCommand.Available = false;
            createProductCommand.VisibleToConsumers = false;
            createProductCommand.VisibleToStores = false;
            createProductCommand.SkipUpdateProducerTags = true;

            return Success(createProductCommand);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Business
{
    public class ExcelProductsImporter : SheaftService, IProductsFileImporter
    {
        public ExcelProductsImporter(ILogger<ExcelProductsImporter> logger) : base(logger)
        {
        }
        
        public Task<Result<IEnumerable<ImportedProductDto>>> ImportAsync(byte[] productsFile, IEnumerable<KeyValuePair<Guid, string>> tags, CancellationToken token)
        {
            using var stream = new MemoryStream(productsFile);
            using var package = new ExcelPackage(stream);
            
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
                return Task.FromResult(Failure<IEnumerable<ImportedProductDto>>(new ValidationException(MessageKind.ImportProduct_Missing_Tab)));

            var productsToImport = new List<ImportedProductDto>();
            for (var i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                var command = CreateProductCommandFromRowData(worksheet, tags, i);
                if (!command.Succeeded)
                    throw command.Exception;
                        
                productsToImport.Add(command.Data);
            }

            return Task.FromResult(Success(productsToImport.AsEnumerable()));
        }

        private Result<ImportedProductDto> CreateProductCommandFromRowData(ExcelWorksheet worksheet, IEnumerable<KeyValuePair<Guid, string>> tags, int i)
        {
            var nameStr = worksheet.Cells[i, 2].GetValue<string>();
            if (string.IsNullOrWhiteSpace(nameStr))
                return Failure<ImportedProductDto>(
                    new ValidationException(MessageKind.CreateProduct_Name_Required_Line, i));

            var createProductCommand = new ImportedProductDto
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
                return Failure<ImportedProductDto>(
                    new ValidationException(MessageKind.CreateProduct_WholeSalePrice_Invalid_Line, i));
            else
                createProductCommand.WholeSalePricePerUnit = wholeSalePrice;

            if (!decimal.TryParse(vatStr, NumberStyles.Any, new CultureInfo("en-US"), out decimal vat) ||
                (vat != 5.5m && vat != 10m && vat != 20m))
                return Failure<ImportedProductDto>(new ValidationException(MessageKind.CreateProduct_Vat_Invalid_Line,
                    i));
            else
                createProductCommand.Vat = vat;

            if (string.IsNullOrWhiteSpace(quantityPerUnitStr))
                quantityPerUnitStr = "1";

            if (!decimal.TryParse(quantityPerUnitStr, NumberStyles.Any, new CultureInfo("en-US"),
                out decimal qtyPerUnit))
                return Failure<ImportedProductDto>(
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
                    return Failure<ImportedProductDto>(new ValidationException(MessageKind.CreateProduct_UnitKind_Invalid_Line, i));
                
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

            createProductCommand.Tags = tags
                .Where(t => tagsAsStr.Contains(t.Value))
                .Select(t => t.Key)
                .ToList();
            
            createProductCommand.Available = false;
            createProductCommand.VisibleToConsumers = true;
            createProductCommand.VisibleToStores = true;

            return Success(createProductCommand);
        }
    }
}
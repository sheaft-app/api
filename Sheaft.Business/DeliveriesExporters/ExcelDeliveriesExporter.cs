﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Extensions;

namespace Sheaft.Business.DeliveriesExporters
{
    public class ExcelDeliveriesExporter : SheaftService, IDeliveriesFileExporter
    {
        public ExcelDeliveriesExporter(ILogger<ExcelDeliveriesExporter> logger) : base(logger)
        {
        }

        public async Task<Result<ResourceExportDto>> ExportAsync(RequestUser requestUser, DateTimeOffset @from,
            DateTimeOffset to, IQueryable<Delivery> deliveriesQuery,
            CancellationToken token)
        {
            using (var stream = new MemoryStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var deliveries = await deliveriesQuery.ToListAsync(token);
                    var data = CreateExcelDeliveriesFile(package, requestUser, from, to, deliveries, token);

                    return Success(new ResourceExportDto
                        {Data = data, Extension = "xlsx", MimeType = "application/ms-excel"});
                }
            }
        }

        private byte[] CreateExcelDeliveriesFile(ExcelPackage package,
            RequestUser user, DateTimeOffset from, DateTimeOffset to, IEnumerable<Delivery> deliveries,
            CancellationToken token)
        {
            var worksheet = package.Workbook.Worksheets.Add($"Ventes {from:dd-MM-yyyy} au {to:dd-MM-yyyy}");

            worksheet.Cells[1, 1].Value = "Client";
            worksheet.Cells[1, 2].Value = "Livraisons";
            worksheet.Cells[1, 3].Value = "Commandes";
            worksheet.Cells[1, 4].Value = "Lots";
            worksheet.Cells[1, 5].Value = "Référence";
            worksheet.Cells[1, 6].Value = "Produit";
            worksheet.Cells[1, 7].Value = "PU HT";
            worksheet.Cells[1, 8].Value = "TVA";
            worksheet.Cells[1, 9].Value = "Quantité";
            worksheet.Cells[1, 10].Value = "Total HT";
            worksheet.Cells[1, 11].Value = "Total TTC";

            SetStyle(worksheet.Cells[1, 1, 1, 11], 
                "#90bf72", 
                "#ffffff", 
                true, 
                ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Center);
            
            var i = 2;

            var groupedDeliveries = deliveries
                .GroupBy(d => new {d.ClientId, d.Client})
                .OrderBy(c => c.Key.Client);

            foreach (var groupedDelivery in groupedDeliveries)
            {
                var j = i;
                var clientDeliveries = groupedDelivery
                    .OrderBy(c => c.Reference)
                    .Select(c => $"{c.Reference.AsDeliveryIdentifier()} ({c.DeliveredOn:dd/MM/yyyy})")
                    .ToList();

                var clientPurchaseOrders = groupedDelivery
                    .SelectMany(gd => gd.PurchaseOrders)
                    .ToList();

                var clientBatches = clientPurchaseOrders
                    .SelectMany(cp => cp.Picking.PreparedProducts
                        .SelectMany(pp => pp.Batches
                            .Select(b => b.Batch)
                            .OrderBy(po => po.Number)
                        )).ToList();

                worksheet.Cells[i, 1].Value = groupedDelivery.Key.Client;
                worksheet.Cells[i, 2].Value = string.Join(", ", clientDeliveries);
                worksheet.Cells[i, 3].Value = string.Join(", ", clientPurchaseOrders
                    .OrderBy(po => po.Reference)
                    .Select(po => $"{po.Reference.AsPurchaseOrderIdentifier()} ({po.CreatedOn:dd/MM/yyyy})"));
                worksheet.Cells[i, 4].Value = string.Join(", ",
                    clientBatches.Select(po => $"{po.Number} - {po.DLC:dd/MM/yyyy}{po.DDM:dd/MM/yyyy}"));

                var clientGroupedProducts = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .GroupBy(p => p.ProductId)
                    .ToList();

                foreach (var groupedProduct in clientGroupedProducts)
                {
                    var product = groupedProduct.First();
                    worksheet.Cells[i, 5].Value = product.Reference;
                    worksheet.Cells[i, 6].Value = $"{product.Name} - {product.GetConditioning()}";
                    worksheet.Cells[i, 7].Value = product.UnitWholeSalePrice;
                    worksheet.Cells[i, 8].Value = product.Vat;
                    worksheet.Cells[i, 9].Value = groupedProduct.Sum(p => p.Quantity);
                    worksheet.Cells[i, 10].Value = groupedProduct.Sum(p => p.TotalProductWholeSalePrice);
                    worksheet.Cells[i, 11].Value = groupedProduct.Sum(p => p.TotalProductOnSalePrice);
                    i++;
                }

                var productsCount = clientGroupedProducts.Count;

                var clientGroupedProductsReturnables = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .Where(p => p.HasReturnable)
                    .GroupBy(p => p.ProductId)
                    .ToList();

                foreach (var groupedProductReturnable in clientGroupedProductsReturnables)
                {
                    var returnable = groupedProductReturnable.First();
                    worksheet.Cells[i, 5].Value = "Consignes déposées";
                    worksheet.Cells[i, 6].Value = $"{returnable.ReturnableName}";
                    worksheet.Cells[i, 7].Value = groupedProductReturnable.Sum(p => p.Quantity);
                    worksheet.Cells[i, 8].Value = returnable.ReturnableWholeSalePrice;
                    worksheet.Cells[i, 9].Value = returnable.ReturnableVat;
                    worksheet.Cells[i, 10].Value = groupedProductReturnable.Sum(p => p.TotalReturnableWholeSalePrice);
                    worksheet.Cells[i, 11].Value = groupedProductReturnable.Sum(p => p.TotalReturnableOnSalePrice);
                    i++;
                }

                productsCount += clientGroupedProductsReturnables.Count;
                
                var clientGroupedReturnedReturnables = groupedDelivery
                    .SelectMany(gd => gd.ReturnedReturnables)
                    .GroupBy(p => p.ReturnableId)
                    .ToList();

                foreach (var groupedReturnedReturnable in clientGroupedReturnedReturnables)
                {
                    var returnedReturnable = groupedReturnedReturnable.First();
                    worksheet.Cells[i, 5].Value = "Consignes récupérées";
                    worksheet.Cells[i, 6].Value = $"{returnedReturnable.Name}";
                    worksheet.Cells[i, 7].Value = groupedReturnedReturnable.Sum(p => p.Quantity);
                    worksheet.Cells[i, 8].Value = returnedReturnable.UnitWholeSalePrice;
                    worksheet.Cells[i, 9].Value = returnedReturnable.Vat;
                    worksheet.Cells[i, 10].Value = groupedReturnedReturnable.Sum(p => p.TotalWholeSalePrice);
                    worksheet.Cells[i, 11].Value = groupedReturnedReturnable.Sum(p => p.TotalOnSalePrice);
                    i++;
                }
                
                productsCount += clientGroupedReturnedReturnables.Count;

                SetStyle(worksheet.Cells[i-productsCount, 7, i, 7], 
                    "#d5e3f5", 
                    null, 
                    false, 
                    ExcelVerticalAlignment.Center,
                    ExcelHorizontalAlignment.Right);
                
                SetStyle(worksheet.Cells[i-productsCount, 10, i, 11], 
                    "#d5e3f5", 
                    null, 
                    false, 
                    ExcelVerticalAlignment.Center,
                    ExcelHorizontalAlignment.Right);

                worksheet.Cells[i, 5].Value = "Total HT";
                worksheet.Cells[i, 5, i, 10].Merge = true;

                var productsWholeSalePrice = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .Sum(p => p.TotalProductWholeSalePrice);

                var returnablesWholeSalePrice =
                    groupedDelivery
                        .SelectMany(gd => gd.Products)
                        .Sum(p => p.TotalReturnableWholeSalePrice ?? 0);

                var returnedReturnablesWholeSalePrice =
                    groupedDelivery
                        .SelectMany(gd => gd.ReturnedReturnables)
                        .Sum(p => p.TotalWholeSalePrice);

                worksheet.Cells[i, 11].Value =
                    Math.Round(productsWholeSalePrice + returnablesWholeSalePrice + returnedReturnablesWholeSalePrice,
                        2);

                i++;
                worksheet.Cells[i, 5].Value = "Total TVA";
                worksheet.Cells[i, 5, i, 10].Merge = true;

                var productsVatPrice = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .Sum(p => p.TotalProductVatPrice);

                var returnablesVatPrice = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .Sum(p => p.TotalReturnableVatPrice ?? 0);

                var returnedReturnablesVatPrice = groupedDelivery
                    .SelectMany(gd => gd.ReturnedReturnables)
                    .Sum(p => p.TotalVatPrice);

                worksheet.Cells[i, 11].Value =
                    Math.Round(productsVatPrice + returnablesVatPrice + returnedReturnablesVatPrice, 2);

                i++;
                worksheet.Cells[i, 5].Value = "Total TTC";
                worksheet.Cells[i, 5, i, 10].Merge = true;

                var productsOnSalePrice = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .Sum(p => p.TotalProductOnSalePrice);

                var returnablesOnSalePrice = groupedDelivery
                    .SelectMany(gd => gd.Products)
                    .Sum(p => p.TotalReturnableOnSalePrice ?? 0);

                var returnedReturnablesOnSalePrice = groupedDelivery
                    .SelectMany(gd => gd.ReturnedReturnables)
                    .Sum(p => p.TotalOnSalePrice);

                worksheet.Cells[i, 11].Value =
                    Math.Round(productsOnSalePrice + returnablesOnSalePrice + returnedReturnablesOnSalePrice, 2);
                
                SetStyle(worksheet.Cells[i-2, 5, i, 11], 
                    "#adcff7", 
                    null, 
                    true, 
                    ExcelVerticalAlignment.Center,
                    ExcelHorizontalAlignment.Right);

                worksheet.Cells[j, 1, i, 1].Merge = true;
                worksheet.Cells[j, 2, i, 2].Merge = true;
                worksheet.Cells[j, 3, i, 3].Merge = true;
                worksheet.Cells[j, 4, i, 4].Merge = true;
                i++;
            }
            
            SetStyle(worksheet.Cells[2, 1, i - 1, 4], 
                null, 
                null, 
                false, 
                ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Left);

            worksheet.Row(1).Height = 35;
            worksheet.Column(1).Width = 20;
            worksheet.Column(1).Style.Font.Bold = true;
            worksheet.Column(2).Width = 25;
            worksheet.Column(3).Width = 25;
            worksheet.Column(4).Width = 25;
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).AutoFit();
            worksheet.Column(10).AutoFit();
            worksheet.Column(11).AutoFit();

            for (var row = 1; row < i; row++)
                for (var col = 1; col < 12; col++)
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            return package.GetAsByteArray();
        }

        private static void SetStyle(ExcelRange range, string backcolor = null, string fontcolor = null,
            bool? bold = null, ExcelVerticalAlignment? verticalAlignment = null,
            ExcelHorizontalAlignment? horizontalAlignment = null)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.WrapText = true;

            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(backcolor ?? "#ffffff"));
            range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(fontcolor ?? "#000000"));
            range.Style.Font.Bold = bold ?? false;
            range.Style.VerticalAlignment = verticalAlignment ?? ExcelVerticalAlignment.Center;
            range.Style.HorizontalAlignment = horizontalAlignment ?? ExcelHorizontalAlignment.Left;
        }
    }
}
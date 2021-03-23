﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Business
{
    public class LfdpPickingOrdersExporter : SheaftService, IPickingOrdersFileExporter
    {
        private readonly IAppDbContext _context;

        public LfdpPickingOrdersExporter(
            IAppDbContext context,
            ILogger<LfdpPickingOrdersExporter> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<ResourceExportDto>> ExportAsync(RequestUser user,
            IQueryable<PurchaseOrder> purchaseOrdersQuery, CancellationToken token)
        {
            using (var stream = new MemoryStream())
            {
                using (var excelPkg = new ExcelPackage())
                {
                    var purchaseOrders = await purchaseOrdersQuery.ToListAsync(token);
                    var storeDeliveryDays = purchaseOrders
                        .Where(po => po.Sender.Kind == ProfileKind.Store)
                        .GroupBy(po => po.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek);

                    var products = await _context.Products
                        .Where(p =>
                            !p.RemovedOn.HasValue
                            && p.Producer.Id == user.Id)
                        .ToListAsync(token);

                    foreach (var storeDeliveryDay in storeDeliveryDays)
                    {
                        var worksheet =
                            excelPkg.Workbook.Worksheets.Add("Commandes du " +
                                                             DayHelper.GetDayFromEnum(storeDeliveryDay.Key));
                        var clients = await _context.Agreements
                            .Where(u =>
                                !u.RemovedOn.HasValue
                                && u.Delivery.Producer.Id == user.Id
                                && u.Delivery.Kind == DeliveryKind.ProducerToStore
                                && u.Delivery.OpeningHours.Any(oh => oh.Day == storeDeliveryDay.Key))
                            .Select(c => c.Store.Name)
                            .ToListAsync(token);

                        var storesPurchaseOrders = storeDeliveryDay.Select(d => d).ToList();
                        PopulateWorksheetWithPurchaseOrders(worksheet, storeDeliveryDay.Key, clients, products,
                            storesPurchaseOrders);
                    }

                    var consumerDeliveryDays = purchaseOrders
                        .Where(po => po.Sender.Kind == ProfileKind.Consumer)
                        .GroupBy(po => po.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek)
                        .ToList();

                    if (consumerDeliveryDays.Any())
                    {
                        var clients = consumerDeliveryDays.SelectMany(cpo => cpo.Select(c => c.Sender.Name)).ToList();
                        foreach (var consumerDeliveryDay in consumerDeliveryDays)
                        {
                            var worksheet = excelPkg.Workbook.Worksheets.Add("Commandes consommateurs du " +
                                                                             DayHelper.GetDayFromEnum(
                                                                                 consumerDeliveryDay.Key));
                            var consumersPurchaseOrders = consumerDeliveryDay.Select(d => d).ToList();
                            PopulateWorksheetWithPurchaseOrders(worksheet, consumerDeliveryDay.Key, clients, products,
                                consumersPurchaseOrders);
                        }
                    }

                    return Success(new ResourceExportDto
                        {Data = excelPkg.GetAsByteArray(), Extension = "xlsx", MimeType = "application/ms-excel"});
                }
            }
        }

        private void PopulateWorksheetWithPurchaseOrders(ExcelWorksheet worksheet, DayOfWeek day,
            IEnumerable<string> clients,
            IEnumerable<Product> existingProducts, IEnumerable<PurchaseOrder> purchaseOrders)
        {
            AddHeadersRow(worksheet, DayHelper.GetDayFromEnum(day), clients);
            AddProductsRows(worksheet, clients, existingProducts, purchaseOrders);
        }

        private void AddHeadersRow(ExcelWorksheet worksheet, string day, IEnumerable<string> clients)
        {
            using (var rng = worksheet.Cells[1, 1])
            {
                rng.Value = day;
                rng.AutoFitColumns(25);
                UpdateRngHeader(rng);
            }

            var pClient = 1;
            foreach (var client in clients)
            {
                using (var rng = worksheet.Cells[1, pClient * 2, 1, pClient * 2 + 1])
                {
                    rng.Value = client;
                    rng.Merge = true;
                    rng.AutoFitColumns(15);
                    UpdateRngHeader(rng);
                }

                pClient++;
            }

            using (var rng = worksheet.Cells[1, clients.Count() * 2 + 2])
            {
                rng.Value = "Total";
                rng.AutoFitColumns(15);
                UpdateRngHeader(rng);
            }


            using (var rng = worksheet.Cells[1, clients.Count() * 2 + 3])
            {
                rng.AutoFitColumns(25);
                UpdateRngHeader(rng);
            }
        }

        private int AddProductsRows(ExcelWorksheet worksheet, IEnumerable<string> clients,
            IEnumerable<Product> existingProducts, IEnumerable<PurchaseOrder> purchaseOrders)
        {
            var grandTotal = 0;

            var pRow = 2;
            foreach (var product in existingProducts)
            {
                using (var rng = worksheet.Cells[pRow, 1])
                {
                    rng.Value = product.Name;
                    rng.AutoFitColumns(25);
                    UpdateRngProductLeft(rng);
                }

                var total = 0;

                var pClient = 1;
                foreach (var client in clients)
                {
                    var current = 0;
                    var order = purchaseOrders.FirstOrDefault(o => o.Sender.Name == client);
                    var productOrder = order?.Products.FirstOrDefault(op => op.Id == product.Id);

                    current += productOrder?.Quantity ?? 0;
                    total += current;

                    using (var rng = worksheet.Cells[pRow, pClient * 2, pRow, pClient * 2 + 1])
                    {
                        rng.Value = current;
                        rng.Merge = true;
                        rng.AutoFitColumns(15);
                        UpdateRngValue(rng);
                    }

                    pClient++;
                }

                grandTotal += total;

                using (var rng = worksheet.Cells[pRow, clients.Count() * 2 + 2])
                {
                    rng.Value = total;
                    rng.AutoFitColumns(15);
                    UpdateRngValue(rng);
                }

                using (var rng = worksheet.Cells[pRow, clients.Count() * 2 + 3])
                {
                    rng.Value = product.Name;
                    rng.AutoFitColumns(25);
                    UpdateRngProductRight(rng);
                }

                pRow++;
            }

            return grandTotal;
        }

        private void UpdateRngProductLeft(ExcelRange rng)
        {
            rng.Style.Font.Bold = true;
            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(237, 237, 237));
            rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);

            UpdateBorders(rng);
        }

        private void UpdateRngProductRight(ExcelRange rng)
        {
            rng.Style.Font.Bold = true;
            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(237, 237, 237));
            rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);

            UpdateBorders(rng);
        }

        private void UpdateRngHeader(ExcelRange rng)
        {
            rng.Style.Font.Bold = true;
            rng.Style.Font.Size = 12;
            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));
            rng.Style.Font.Color.SetColor(System.Drawing.Color.White);

            UpdateBorders(rng);
        }

        private void UpdateRngValue(ExcelRange rng)
        {
            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            UpdateBorders(rng);
        }

        private void UpdateBorders(ExcelRange rng)
        {
            rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
        }

        private static class DayHelper
        {
            public static string GetDayFromEnum(DayOfWeek day)
            {
                switch (day)
                {
                    case DayOfWeek.Monday:
                        return "Lundi";
                    case DayOfWeek.Tuesday:
                        return "Mardi";
                    case DayOfWeek.Wednesday:
                        return "Mercredi";
                    case DayOfWeek.Thursday:
                        return "Jeudi";
                    case DayOfWeek.Friday:
                        return "Vendredi";
                    case DayOfWeek.Saturday:
                        return "Samedi";
                    default:
                        return "Dimanche";
                }
            }
        }
    }
}
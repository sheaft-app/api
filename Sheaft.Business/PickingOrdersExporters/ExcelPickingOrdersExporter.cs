using System;
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

namespace Sheaft.Business.PickingOrdersExporters
{
    public class ExcelPickingOrdersExporter : SheaftService, IPickingOrdersFileExporter
    {
        public ExcelPickingOrdersExporter(ILogger<ExcelPickingOrdersExporter> logger) : base(logger)
        {
        }
        
        public async Task<Result<ResourceExportDto>> ExportAsync(RequestUser user, IQueryable<PurchaseOrder> purchaseOrdersQuery, CancellationToken token)
        {
            using (var stream = new MemoryStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var purchaseOrders = await purchaseOrdersQuery.ToListAsync(token);
                    var data = CreateExcelPickingFile(package, purchaseOrders);
                    return Success(new ResourceExportDto{ Data = data, Extension = "xlsx", MimeType = "application/ms-excel"});
                }
            }
        }

        private byte[] CreateExcelPickingFile(ExcelPackage package, IEnumerable<PurchaseOrder> purchaseOrders)
        {
            var products = new List<LightProduct>();
            foreach (var purchaseOrder in purchaseOrders)
                products.AddRange(purchaseOrder.Products.Select(GetLightProduct));

            var subsetOrders = purchaseOrders.Select(o => new LightOrder
            {
                Reference = o.Reference.AsPurchaseOrderIdentifier(),
                ExpectedDeliveryDate = o.ExpectedDelivery.ExpectedDeliveryDate.ToString("dd/MM/yyyy"),
                SenderId = o.ClientId,
                SenderName = o.SenderInfo.Name,
                Products = o.Products.Select(GetLightProduct)
            });

            var groupedOrders = subsetOrders.OrderBy(c => c.SenderName)
                .GroupBy(lc => new {Id = lc.SenderId, Name = lc.SenderName});

            var worksheet = package.Workbook.Worksheets.Add("Préparation");
            var lastRow = WriteProductsColumn(worksheet, products);
            var productsCount = lastRow - PickingOrderFileSettings.PRODUCTS_START_ROW;

            var clientColumn = PickingOrderFileSettings.CLIENTS_START_COLUMN;
            var columnToHide = new List<int>();
            foreach (var orders in groupedOrders)
            {
                var clientOrdersCount = orders.Count();
                var orderIndex = 1;

                foreach (var order in orders)
                {
                    var orderColumn = clientColumn + orderIndex - 1;

                    WriteColumnOrderHeader(worksheet, orderColumn, $"{order.Reference} ({order.ExpectedDeliveryDate})");
                    WriteColumnProductsQuantity(worksheet, productsCount, orderColumn, order.Products);
                    WriteColumnTotalRow(worksheet, lastRow, orderColumn);

                    if (clientOrdersCount > 1)
                        columnToHide.Add(orderColumn);

                    worksheet.Column(orderColumn).Width = 15;
                    orderIndex++;
                }

                clientOrdersCount += WriteColumnSubTotalRows(worksheet, clientColumn, clientOrdersCount, lastRow);
                WriteColumnClientHeader(worksheet, clientColumn, clientOrdersCount, orders.Key.Name);

                clientColumn += clientOrdersCount;
            }

            WriteColumnTotal(worksheet, clientColumn, productsCount);
            WriteTitleRow(worksheet, clientColumn);

            if (columnToHide.Any())
                foreach (var col in columnToHide)
                    worksheet.Column(col).Hidden = true;

            var range = worksheet.Cells[1, 1, lastRow, clientColumn];

            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Row(1).Height = 30;
            worksheet.Row(2).Height = 30;
            worksheet.Row(3).Height = 30;
            worksheet.Column(1).Width = 25;

            worksheet.Calculate();
            return package.GetAsByteArray();
        }

        private static void WriteTitleRow(ExcelWorksheet worksheet, int clientColumn)
        {
            var range = worksheet.Cells[1, 1, 1, clientColumn];
            range.Merge = true;
            range.Value = "Préparation des commandes";
            SetStyle(range, "#548235", "#ffffff", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);
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

        private void WriteColumnTotal(ExcelWorksheet worksheet, int clientColumn, int productsCount)
        {
            var range = worksheet.Cells[PickingOrderFileSettings.CLIENT_NAME_ROW, clientColumn,
                PickingOrderFileSettings.CLIENT_NAME_ROW + 1, clientColumn];
            range.Merge = true;
            range.Value = "Total";

            SetStyle(range, "#DDEBF7", "#000000", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);

            for (var i = 0; i < productsCount; i++)
            {
                var currentProductRow = PickingOrderFileSettings.PRODUCTS_START_ROW + i;
                var rowRanges = new List<string>();
                for (var j = PickingOrderFileSettings.CLIENTS_START_COLUMN; j < clientColumn; j++)
                {
                    if (string.IsNullOrWhiteSpace(worksheet.Cells[currentProductRow, j].Formula))
                        rowRanges.Add(worksheet.Cells[currentProductRow, j].Address);
                }

                var rowFormula = "";
                foreach (var rowRange in rowRanges)
                {
                    if (string.IsNullOrWhiteSpace(rowFormula))
                        rowFormula = $"SUM({rowRange}";
                    else
                        rowFormula += $",{rowRange}";
                }

                if (rowFormula.Length > 0)
                    rowFormula += ")";

                var rowValueRange = worksheet.Cells[currentProductRow, clientColumn];
                rowValueRange.Formula = rowFormula;
                SetStyle(rowValueRange, "#DDEBF7", "#000000", true, ExcelVerticalAlignment.Center,
                    ExcelHorizontalAlignment.Right);
            }

            var totalRows = PickingOrderFileSettings.PRODUCTS_START_ROW + productsCount;
            WriteColumnTotalRow(worksheet, totalRows, clientColumn, true);
        }

        private void WriteColumnProductsQuantity(ExcelWorksheet worksheet, int productsCount, int currentColumn,
            IEnumerable<LightProduct> orderProducts)
        {
            if (!orderProducts.Any())
                return;

            foreach (var orderProduct in orderProducts)
            {
                for (var i = 0; i < productsCount; i++)
                {
                    var currentProductRow = PickingOrderFileSettings.PRODUCTS_START_ROW + i;
                    var productName = worksheet.Cells[currentProductRow, PickingOrderFileSettings.PRODUCT_COLUMN].Text;
                    if (productName != orderProduct.Name)
                        continue;

                    worksheet.Cells[currentProductRow, currentColumn].Value = orderProduct.Quantity;
                    break;
                }
            }
        }

        private void WriteColumnOrderHeader(ExcelWorksheet worksheet, int currentColumn, string label)
        {
            var range = worksheet.Cells[PickingOrderFileSettings.ORDER_REFERENCE_ROW, currentColumn];
            range.Value = label;

            SetStyle(range, "#C6E0B4", "#000000", false, ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Center);
        }

        private int WriteProductsColumn(ExcelWorksheet worksheet, IEnumerable<LightProduct> products)
        {
            var clientNameRange = worksheet.Cells[PickingOrderFileSettings.CLIENT_NAME_ROW,
                PickingOrderFileSettings.PRODUCT_COLUMN];
            clientNameRange.Value = "Client";
            SetStyle(clientNameRange, "#A9D08E", "#000000", true, ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Center);

            var referenceRange = worksheet.Cells[PickingOrderFileSettings.ORDER_REFERENCE_ROW,
                PickingOrderFileSettings.PRODUCT_COLUMN];
            referenceRange.Value = "Commande(s)";
            SetStyle(referenceRange, "#C6E0B4", "#000000", false, ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Center);

            var productsName = products.Select(p => p.Name).Distinct().OrderBy(c => c).ToList();

            var row = 0;
            foreach (var productName in productsName)
            {
                var productRange = worksheet.Cells[PickingOrderFileSettings.PRODUCTS_START_ROW + row,
                    PickingOrderFileSettings.PRODUCT_COLUMN];
                productRange.Value = productName;

                SetStyle(productRange, "#E2EFDA", "#000000", false, ExcelVerticalAlignment.Center,
                    ExcelHorizontalAlignment.Left);
                row++;
            }

            var totalRow = PickingOrderFileSettings.PRODUCTS_START_ROW + row;
            var totalRange = worksheet.Cells[totalRow, PickingOrderFileSettings.PRODUCT_COLUMN];
            totalRange.Value = "Total";

            SetStyle(totalRange, "#DDEBF7", "#000000", true, ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Right);

            return totalRow;
        }

        private void WriteColumnClientHeader(ExcelWorksheet worksheet, int startColumn, int ordersCount,
            string label)
        {
            var column = startColumn + ordersCount - 1;

            var clientHeaderRange = worksheet.Cells[PickingOrderFileSettings.CLIENT_NAME_ROW, startColumn,
                PickingOrderFileSettings.CLIENT_NAME_ROW, column];
            clientHeaderRange.Merge = column > 1;
            clientHeaderRange.Value = label;

            worksheet.Row(PickingOrderFileSettings.CLIENT_NAME_ROW).Height = 30;
            SetStyle(clientHeaderRange, "#A9D08E", "#000000", true, ExcelVerticalAlignment.Center,
                ExcelHorizontalAlignment.Center);
        }

        private int WriteColumnSubTotalRows(ExcelWorksheet worksheet, int startColumn, int ordersCount, int row)
        {
            if (ordersCount <= 1)
                return 0;

            var column = startColumn + ordersCount;
            var range = worksheet.Cells[PickingOrderFileSettings.ORDER_REFERENCE_ROW, column];
            range.Value = "Sous total";
            SetStyle(range, "#C6E0B4", "#000000", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);

            for (var i = PickingOrderFileSettings.PRODUCTS_START_ROW; i < row; i++)
            {
                var startCell = worksheet.Cells[i, startColumn];
                var endCell = worksheet.Cells[i, column - 1];
                worksheet.Cells[i, column].Formula = $"SUM({startCell.Address}:{endCell.Address})";
            }

            WriteColumnTotalRow(worksheet, row, column);
            return 1;
        }

        private void WriteColumnTotalRow(ExcelWorksheet worksheet, int row, int column, bool bold = false)
        {
            var startTotalCell = worksheet.Cells[PickingOrderFileSettings.PRODUCTS_START_ROW, column];
            var endTotalCell = worksheet.Cells[row - 1, column];

            var range = worksheet.Cells[row, column];
            range.Formula = $"SUM({startTotalCell.Address}:{endTotalCell.Address})";

            SetStyle(range, "#DDEBF7", "#000000", bold, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Right);
        }

        private LightProduct GetLightProduct(PurchaseOrderProduct p)
        {
            return new LightProduct
            {
                Id = p.ProductId,
                Name = $"{p.Name}{(p.UnitWeight.HasValue && p.UnitWeight > 0 ? " " + p.UnitWeight.Value : "")}",
                Reference = p.Reference,
                Quantity = p.Quantity
            };
        }

        private class LightOrder
        {
            public Guid SenderId { get; set; }
            public string Reference { get; set; }
            public string SenderName { get; set; }
            public string ExpectedDeliveryDate { get; set; }
            public IEnumerable<LightProduct> Products { get; set; }
        }

        private class LightProduct
        {
            public Guid Id { get; set; }
            public string Reference { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }

        private static class PickingOrderFileSettings
        {
            public const int CLIENT_NAME_ROW = 2;
            public const int ORDER_REFERENCE_ROW = CLIENT_NAME_ROW + 1;
            public const int PRODUCTS_START_ROW = ORDER_REFERENCE_ROW + 1;
            public const int PRODUCT_COLUMN = 1;
            public const int CLIENTS_START_COLUMN = 2;
        }
    }
}
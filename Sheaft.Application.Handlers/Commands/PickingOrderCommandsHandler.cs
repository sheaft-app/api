using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class PickingOrderCommandsHandler : ResultsHandler,
        IRequestHandler<QueueExportPickingOrderCommand, Result<Guid>>,
        IRequestHandler<ExportPickingOrderCommand, Result<bool>>
    {
        private readonly IBlobService _blobsService;

        public PickingOrderCommandsHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context, 
            IBlobService blobsService,
            ILogger<PickingOrderCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobsService = blobsService;
        }

        public async Task<Result<Guid>> Handle(QueueExportPickingOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var purchaseOrders = await _context.GetByIdsAsync<PurchaseOrder>(request.PurchaseOrderIds, token);

                var orderIdsToAccept = purchaseOrders.Where(c => c.Status == PurchaseOrderStatus.Waiting).Select(c => c.Id);
                if (orderIdsToAccept.Any())
                {
                    var result = await _mediatr.Process(new AcceptPurchaseOrdersCommand(request.RequestUser) { Ids = orderIdsToAccept }, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);
                }

                var entity = new Job(Guid.NewGuid(), JobKind.CreatePickingFromOrders, request.Name ?? $"Export bon préparation", producer);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new ExportPickingOrderCommand(request.RequestUser) { JobId = entity.Id, PurchaseOrderIds = request.PurchaseOrderIds });

                return Ok(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(ExportPickingOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var job = await _context.GetByIdAsync<Job>(request.JobId, token);

                try
                {
                    var startResult = await _mediatr.Process(new StartJobCommand(request.RequestUser) { Id = job.Id }, token);
                    if (!startResult.Success)
                        throw startResult.Exception;

                    var purchaseOrders = await _context.GetByIdsAsync<PurchaseOrder>(request.PurchaseOrderIds, token);
                    _mediatr.Post(new PickingOrderExportProcessingEvent(request.RequestUser) { JobId = job.Id });

                    using (var stream = new MemoryStream())
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            CreateExcelPickingFile(package, job.Name, purchaseOrders);
                        }

                        var response = await _blobsService.UploadPickingOrderFileAsync(request.RequestUser.Id, job.Id, $"Preparation_{job.CreatedOn:dd-MM-yyyy}.xlsx", stream, token);
                        if (!response.Success)
                            throw response.Exception;

                        var result = await _mediatr.Process(new ProcessPurchaseOrdersCommand(request.RequestUser) { Ids = request.PurchaseOrderIds }, token);
                        if (!result.Success)
                            throw result.Exception;

                        _mediatr.Post(new PickingOrderExportSucceededEvent(request.RequestUser) { JobId = job.Id });
                        return await _mediatr.Process(new CompleteJobCommand(request.RequestUser) { Id = job.Id, FileUrl = response.Data }, token);
                    }
                }
                catch (Exception e)
                {
                    _mediatr.Post(new PickingOrderExportFailedEvent(request.RequestUser) { JobId = job.Id });
                    return await _mediatr.Process(new FailJobCommand(request.RequestUser) { Id = request.JobId, Reason = e.Message }, token);
                }
            });
        }

        #region Picking Implementation

        private class LightOrder
        {
            public Guid SenderId { get; set; }
            public string Reference { get; set; }
            public string SenderName { get; set; }
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

        private void CreateExcelPickingFile(ExcelPackage package, string title,
            IEnumerable<PurchaseOrder> purchaseOrders)
        {
            var products = new List<LightProduct>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                products.AddRange(purchaseOrder.Products.Select(GetLightProduct));
            }

            var subsetOrders = purchaseOrders.Select(o => new LightOrder
            {
                Reference = o.Reference,
                SenderId = o.Sender.Id,
                SenderName = o.Sender.Name,
                Products = o.Products.Select(GetLightProduct)
            });

            var groupedOrders = subsetOrders.OrderBy(c => c.SenderName)
                .GroupBy(lc => new { Id = lc.SenderId, Name = lc.SenderName });

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

                    WriteColumnOrderHeader(worksheet, orderColumn, order.Reference);
                    WriteColumnProductsQuantity(worksheet, productsCount, orderColumn, order.Products);
                    WriteColumnTotalRow(worksheet, lastRow, orderColumn);

                    if (clientOrdersCount > 1)
                        columnToHide.Add(orderColumn);

                    orderIndex++;
                }

                clientOrdersCount += WriteColumnSubTotalRows(worksheet, clientColumn, clientOrdersCount, lastRow);
                WriteColumnClientHeader(worksheet, clientColumn, clientOrdersCount, orders.Key.Name);

                clientColumn += clientOrdersCount;
            }

            WriteColumnTotal(worksheet, clientColumn, productsCount);
            WriteTitleRow(worksheet, title, clientColumn);

            if (columnToHide.Any())
            {
                worksheet.Row(PickingOrderFileSettings.ORDER_REFERENCE_ROW).Hidden = true;
                worksheet.Cells.AutoFitColumns();
                foreach (var col in columnToHide)
                {
                    worksheet.Column(col).Hidden = true;
                }
            }
            else
            {
                worksheet.Cells.AutoFitColumns();
            }

            var range = worksheet.Cells[1, 1, clientColumn, lastRow];

            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Calculate();
            package.Save();
        }

        private static void WriteTitleRow(ExcelWorksheet worksheet, string title, int clientColumn)
        {
            var range = worksheet.Cells[1, 1, 1, clientColumn];
            range.Merge = true;
            range.Value = title;
            worksheet.Row(1).Height = 45;
            SetStyle(range, "#548235", "#ffffff", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);
        }

        private static void SetStyle(ExcelRange range, string backcolor = null, string fontcolor = null, bool? bold = null, ExcelVerticalAlignment? verticalAlignment = null, ExcelHorizontalAlignment? horizontalAlignment = null)
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
            var range = worksheet.Cells[PickingOrderFileSettings.CLIENT_NAME_ROW, clientColumn, PickingOrderFileSettings.CLIENT_NAME_ROW + 1, clientColumn];
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
                SetStyle(rowValueRange, "#DDEBF7", "#000000", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Right);
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

            SetStyle(range, "#C6E0B4", "#000000", false, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);
        }

        private int WriteProductsColumn(ExcelWorksheet worksheet, IEnumerable<LightProduct> products)
        {
            var clientNameRange = worksheet.Cells[PickingOrderFileSettings.CLIENT_NAME_ROW, PickingOrderFileSettings.PRODUCT_COLUMN];
            clientNameRange.Value = "Client";
            SetStyle(clientNameRange, "#A9D08E", "#000000", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);

            var referenceRange = worksheet.Cells[PickingOrderFileSettings.ORDER_REFERENCE_ROW, PickingOrderFileSettings.PRODUCT_COLUMN];
            referenceRange.Value = "Commande(s)";
            SetStyle(clientNameRange, "#C6E0B4", "#000000", false, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);

            var productsName = products.Select(p => p.Name).Distinct().OrderBy(c => c).ToList();

            var row = 0;
            foreach (var productName in productsName)
            {
                var productRange = worksheet.Cells[PickingOrderFileSettings.PRODUCTS_START_ROW + row, PickingOrderFileSettings.PRODUCT_COLUMN];
                productRange.Value = productName;

                SetStyle(productRange, "#E2EFDA", "#000000", false, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Left);
                row++;
            }

            var totalRow = PickingOrderFileSettings.PRODUCTS_START_ROW + row;
            var totalRange = worksheet.Cells[totalRow, PickingOrderFileSettings.PRODUCT_COLUMN];
            totalRange.Value = "Total";

            SetStyle(totalRange, "#DDEBF7", "#000000", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Right);

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

            worksheet.Row(PickingOrderFileSettings.CLIENT_NAME_ROW).Height = 40;
            SetStyle(clientHeaderRange, "#A9D08E", "#000000", true, ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center);
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
                Id = p.Id,
                Name = $"{p.Name}{(p.UnitWeight.HasValue && p.UnitWeight > 0 ? " " + p.UnitWeight.Value : "")}",
                Reference = p.Reference,
                Quantity = p.Quantity
            };
        }

        #endregion
    }
}